using Microsoft.JSInterop;

namespace ChordDirectory.Services;

public enum PlayStyle
{
    /// <summary>All notes at once.</summary>
    Block,

    /// <summary>A fast, rolled strum from the lowest to the highest note.</summary>
    Strum,

    /// <summary>One note at a time, low to high.</summary>
    ArpUp,

    /// <summary>One note at a time, high to low.</summary>
    ArpDown,
}

public sealed record InstrumentInfo(string Id, string Name);

/// <summary>
/// Thin wrapper around the Web Audio engine in wwwroot/js/audio.js.
/// Also tracks which MIDI notes are currently sounding so the keyboard can light them up.
/// </summary>
public sealed class AudioService : IAsyncDisposable
{
    public static IReadOnlyList<InstrumentInfo> Instruments { get; } =
    [
        new InstrumentInfo("piano", "Piano"),
        new InstrumentInfo("epiano", "Electric piano"),
        new InstrumentInfo("organ", "Organ"),
        new InstrumentInfo("synth", "Synth pad"),
    ];

    private readonly IJSRuntime _js;
    private readonly HashSet<int> _active = [];
    private DotNetObjectReference<AudioService>? _self;

    public AudioService(IJSRuntime js) => _js = js;

    public string InstrumentId { get; set; } = "piano";

    /// <summary>Master volume, 0–1.</summary>
    public double Volume { get; set; } = 0.7;

    /// <summary>Delay between notes in arpeggios, in milliseconds.</summary>
    public int ArpeggioStepMs { get; set; } = 220;

    public IReadOnlyCollection<int> ActiveNotes => _active;

    /// <summary>Raised whenever the set of sounding notes changes.</summary>
    public event Action? ActiveNotesChanged;

    public bool IsActive(int midi) => _active.Contains(midi);

    public async Task PlayChordAsync(IEnumerable<int> midiNotes, PlayStyle style = PlayStyle.Block)
    {
        var notes = midiNotes.OrderBy(n => n).ToArray();
        if (notes.Length == 0) return;

        var styleName = style switch
        {
            PlayStyle.Strum => "strum",
            PlayStyle.ArpUp => "arpup",
            PlayStyle.ArpDown => "arpdown",
            _ => "block",
        };

        await SafeAsync(() => _js.InvokeVoidAsync("ChordAudio.playChord",
            notes, styleName, InstrumentId, Volume, ArpeggioStepMs, Self).AsTask());
    }

    public Task PlayNoteAsync(int midi) =>
        SafeAsync(() => _js.InvokeVoidAsync("ChordAudio.playNote", midi, InstrumentId, Volume, Self).AsTask());

    public Task StopAsync() =>
        SafeAsync(() => _js.InvokeVoidAsync("ChordAudio.stopAll").AsTask());

    private DotNetObjectReference<AudioService> Self => _self ??= DotNetObjectReference.Create(this);

    // Audio problems (blocked autoplay, missing Web Audio) should never crash the UI.
    private static async Task SafeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (JSException ex)
        {
            Console.Error.WriteLine($"Audio error: {ex.Message}");
        }
        catch (JSDisconnectedException)
        {
        }
    }

    [JSInvokable]
    public void OnNoteOn(int midi)
    {
        if (_active.Add(midi)) ActiveNotesChanged?.Invoke();
    }

    [JSInvokable]
    public void OnNoteOff(int midi)
    {
        if (_active.Remove(midi)) ActiveNotesChanged?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        _self?.Dispose();
    }
}
