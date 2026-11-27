// Chord Directory – a small Web Audio synth with four instruments.
// Loaded as a classic script so the AudioContext can be unlocked synchronously
// from the very first user gesture (required by Safari and Chrome autoplay policies).
(function () {
    'use strict';

    let ctx = null;
    let bus = null;           // all voices connect here
    const voices = new Set(); // currently sounding voices
    const timers = new Set(); // pending UI callbacks
    const uiActive = new Set();
    let lastDotnet = null;    // .NET callback target, so stopAll() can clear highlights

    const freq = (midi) => 440 * Math.pow(2, (midi - 69) / 12);

    function impulse(c, seconds, decay) {
        const len = Math.floor(c.sampleRate * seconds);
        const buf = c.createBuffer(2, len, c.sampleRate);
        for (let ch = 0; ch < 2; ch++) {
            const d = buf.getChannelData(ch);
            for (let i = 0; i < len; i++) d[i] = (Math.random() * 2 - 1) * Math.pow(1 - i / len, decay);
        }
        return buf;
    }

    function ensure() {
        if (!ctx) {
            const AC = window.AudioContext || window.webkitAudioContext;
            if (!AC) throw new Error('Web Audio is not supported in this browser.');
            ctx = new AC({ latencyHint: 'interactive' });

            const comp = ctx.createDynamicsCompressor();
            comp.threshold.value = -16;
            comp.knee.value = 20;
            comp.ratio.value = 6;
            comp.attack.value = 0.004;
            comp.release.value = 0.25;

            const master = ctx.createGain();
            master.gain.value = 0.9;
            master.connect(comp);
            comp.connect(ctx.destination);

            bus = ctx.createGain();
            bus.connect(master);

            // A touch of room reverb makes the dry synths much more pleasant.
            const verb = ctx.createConvolver();
            verb.buffer = impulse(ctx, 1.8, 3);
            const wet = ctx.createGain();
            wet.gain.value = 0.16;
            bus.connect(verb);
            verb.connect(wet);
            wet.connect(master);
        }
        if (ctx.state === 'suspended') ctx.resume();
        return ctx;
    }

    // Unlock audio on the first gesture, before Blazor's (async) interop call arrives.
    const unlock = () => {
        try { ensure(); } catch (e) { /* reported when playing */ }
        if (ctx && ctx.state === 'running') {
            ['pointerdown', 'keydown', 'touchend'].forEach((ev) => window.removeEventListener(ev, unlock, true));
        }
    };
    ['pointerdown', 'keydown', 'touchend'].forEach((ev) => window.addEventListener(ev, unlock, true));

    // ── Instruments ────────────────────────────────────────────────────────────
    // Each builder creates its nodes, schedules them for [t, t+dur+release] and returns a voice.

    function osc(type, f, t) {
        const o = ctx.createOscillator();
        o.type = type;
        o.frequency.value = f;
        o.start(t);
        return o;
    }

    function finish(voice, oscs, endTime) {
        oscs.forEach((o) => o.stop(endTime));
        oscs[0].onended = () => {
            voices.delete(voice);
            try { voice.out.disconnect(); } catch (e) { /* already gone */ }
        };
        voice.oscs = oscs;
        voices.add(voice);
        return voice;
    }

    const builders = {
        piano(f, t, dur, v) {
            const out = ctx.createGain();
            out.connect(bus);
            const lp = ctx.createBiquadFilter();
            lp.type = 'lowpass';
            lp.Q.value = 0.4;
            lp.frequency.setValueAtTime(Math.min(f * 12, 14000), t);
            lp.frequency.exponentialRampToValueAtTime(Math.min(f * 3, 6000), t + 1.2);
            lp.connect(out);

            const partials = [1, 0.55, 0.3, 0.2, 0.11, 0.07];
            const oscs = partials.map((amp, i) => {
                const o = osc(i === 0 ? 'triangle' : 'sine', f * (i + 1) * (1 + i * 0.0003), t);
                const g = ctx.createGain();
                g.gain.value = amp * (i === 0 ? 0.8 : 1);
                o.connect(g);
                g.connect(lp);
                return o;
            });

            const hold = t + Math.max(dur, 0.9);
            const rel = 0.35;
            out.gain.setValueAtTime(0.0001, t);
            out.gain.exponentialRampToValueAtTime(v, t + 0.006);
            out.gain.exponentialRampToValueAtTime(v * 0.42, t + 0.55);
            out.gain.exponentialRampToValueAtTime(v * 0.2, hold);
            out.gain.exponentialRampToValueAtTime(0.0001, hold + rel);
            return finish({ out }, oscs, hold + rel + 0.05);
        },

        epiano(f, t, dur, v) {
            const out = ctx.createGain();
            out.connect(bus);
            const carrier = osc('sine', f, t);
            const mod = osc('sine', f, t);
            const modGain = ctx.createGain();
            modGain.gain.setValueAtTime(f * 2.4, t);
            modGain.gain.exponentialRampToValueAtTime(f * 0.3, t + 0.9);
            mod.connect(modGain);
            modGain.connect(carrier.frequency);
            carrier.connect(out);

            // Bright "tine" at the attack
            const tine = osc('sine', f * 7, t);
            const tg = ctx.createGain();
            tg.gain.setValueAtTime(v * 0.12, t);
            tg.gain.exponentialRampToValueAtTime(0.0001, t + 0.18);
            tine.connect(tg);
            tg.connect(out);

            const hold = t + Math.max(dur, 0.9);
            const rel = 0.3;
            out.gain.setValueAtTime(0.0001, t);
            out.gain.exponentialRampToValueAtTime(v, t + 0.008);
            out.gain.exponentialRampToValueAtTime(v * 0.45, t + 0.7);
            out.gain.exponentialRampToValueAtTime(v * 0.25, hold);
            out.gain.exponentialRampToValueAtTime(0.0001, hold + rel);
            return finish({ out }, [carrier, mod, tine], hold + rel + 0.05);
        },

        organ(f, t, dur, v) {
            const out = ctx.createGain();
            out.connect(bus);
            const bars = [[0.5, 0.35], [1, 0.8], [2, 0.55], [3, 0.3], [4, 0.2]];
            const oscs = bars.map(([mult, amp]) => {
                const o = osc('sine', f * mult, t);
                const g = ctx.createGain();
                g.gain.value = amp * 0.5;
                o.connect(g);
                g.connect(out);
                return o;
            });
            const hold = t + Math.max(dur, 0.4);
            const rel = 0.14;
            out.gain.setValueAtTime(0.0001, t);
            out.gain.linearRampToValueAtTime(v * 0.8, t + 0.02);
            out.gain.setValueAtTime(v * 0.8, hold);
            out.gain.exponentialRampToValueAtTime(0.0001, hold + rel);
            return finish({ out }, oscs, hold + rel + 0.05);
        },

        synth(f, t, dur, v) {
            const out = ctx.createGain();
            out.connect(bus);
            const lp = ctx.createBiquadFilter();
            lp.type = 'lowpass';
            lp.Q.value = 2;
            lp.frequency.setValueAtTime(Math.min(f * 8, 9000), t);
            lp.frequency.exponentialRampToValueAtTime(Math.min(f * 2.2 + 300, 3000), t + 0.9);
            lp.connect(out);
            const oscs = [-9, 0, 9].map((cents) => {
                const o = osc('sawtooth', f, t);
                o.detune.value = cents;
                const g = ctx.createGain();
                g.gain.value = 0.33;
                o.connect(g);
                g.connect(lp);
                return o;
            });
            const hold = t + Math.max(dur, 0.6);
            const rel = 0.5;
            out.gain.setValueAtTime(0.0001, t);
            out.gain.linearRampToValueAtTime(v * 0.9, t + 0.05);
            out.gain.setTargetAtTime(v * 0.6, t + 0.05, 0.3);
            out.gain.setValueAtTime(v * 0.6, hold);
            out.gain.exponentialRampToValueAtTime(0.0001, hold + rel);
            return finish({ out }, oscs, hold + rel + 0.05);
        },
    };

    function voice(midi, t, dur, v, instrument) {
        const build = builders[instrument] || builders.piano;
        return build(freq(midi), t, dur, v);
    }

    // ── UI notifications (so the keyboard can light up in sync) ───────────────

    function later(ms, fn) {
        const id = setTimeout(() => { timers.delete(id); fn(); }, Math.max(0, ms));
        timers.add(id);
    }

    function notify(dotnet, method, midi) {
        if (!dotnet) return;
        try { dotnet.invokeMethodAsync(method, midi); } catch (e) { /* component gone */ }
    }

    function schedule(dotnet, midi, startSec, litSec) {
        const delay = (startSec - ctx.currentTime) * 1000;
        later(delay, () => { uiActive.add(midi); notify(dotnet, 'OnNoteOn', midi); });
        later(delay + litSec * 1000, () => { uiActive.delete(midi); notify(dotnet, 'OnNoteOff', midi); });
    }

    // ── Public API ─────────────────────────────────────────────────────────────

    function stopAll(dotnet) {
        if (!ctx) return;
        dotnet = dotnet || lastDotnet;
        const now = ctx.currentTime;
        timers.forEach(clearTimeout);
        timers.clear();
        voices.forEach((vc) => {
            try {
                vc.out.gain.cancelScheduledValues(now);
                vc.out.gain.setTargetAtTime(0.0001, now, 0.04);
                vc.oscs.forEach((o) => { try { o.stop(now + 0.3); } catch (e) { /* already stopped */ } });
            } catch (e) { /* ignore */ }
        });
        if (dotnet) uiActive.forEach((m) => notify(dotnet, 'OnNoteOff', m));
        uiActive.clear();
    }

    /**
     * @param {number[]} notes  MIDI note numbers
     * @param {string} style    block | strum | arpup | arpdown
     * @param {string} instrument
     * @param {number} volume   0..1
     * @param {number} stepMs   arpeggio spacing
     */
    function playChord(notes, style, instrument, volume, stepMs, dotnet) {
        ensure();
        lastDotnet = dotnet || lastDotnet;
        stopAll(dotnet);

        const sorted = notes.slice().sort((a, b) => a - b);
        const order = style === 'arpdown' ? sorted.slice().reverse() : sorted;
        const n = order.length;
        const step = style === 'block' ? 0 : style === 'strum' ? 0.04 : Math.max(0.05, stepMs / 1000);
        const start = ctx.currentTime + 0.04;
        const v = volume * 0.5 / Math.sqrt(n);

        order.forEach((midi, i) => {
            const t = start + i * step;
            // Arpeggiated notes keep ringing until the end of the figure, like a held pedal.
            const dur = style === 'arpup' || style === 'arpdown'
                ? (n - 1 - i) * step + 1.4
                : 2.4 - i * step;
            voice(midi, t, dur, v, instrument);
            schedule(dotnet, midi, t, Math.min(dur, 1.1));
        });
    }

    function playNote(midi, instrument, volume, dotnet) {
        ensure();
        lastDotnet = dotnet || lastDotnet;
        const t = ctx.currentTime + 0.01;
        voice(midi, t, 1.4, volume * 0.45, instrument);
        schedule(dotnet, midi, t, 0.7);
    }

    window.ChordAudio = { playChord, playNote, stopAll };
})();
