using ChordDirectory.Music;

// Checks the music theory without a browser. The exit code is the number of failed checks.
var failures = 0;
var checks = 0;
void Check(bool cond, string what)
{
    checks++;
    if (!cond) { failures++; Console.WriteLine("FAIL: " + what); }
}

Root R(string name) => Roots.FromName(name) ?? throw new ArgumentException(name);
ChordType T(string id) => ChordCatalog.Find(id) ?? throw new ArgumentException(id);
string Notes(string root, string type) => string.Join(" ", new Chord(R(root), T(type)).Notes.Select(n => n.Name));

// ---------- Spelling ----------
Check(Notes("C", "major") == "C E G", "C major");
Check(Notes("Db", "maj7") == "D♭ F A♭ C", "D♭maj7 is spelled with F, not E♯");
Check(Notes("F#", "minor") == "F♯ A C♯", "F♯ minor");
Check(Notes("C", "dim7") == "C E♭ G♭ B♭♭", "Cdim7 has a double-flat seventh");
Check(Notes("Eb", "m7b5") == "E♭ G♭ B♭♭ D♭", "E♭ half-diminished");
Check(Notes("G", "13") == "G B D F A E", "G13");
Check(Notes("B", "7s9") == "B D♯ F♯ A Cx", "B7♯9 has a double-sharp ninth");

// ---------- Catalogue ----------
Check(ChordCatalog.All.Count == 38, "38 chord types");
Check(ChordCatalog.All.Select(t => t.Id).Distinct().Count() == ChordCatalog.All.Count, "chord ids are unique");
Check(ChordCatalog.All.All(t => ChordCatalog.Categories.Contains(t.Category)), "every chord type has a known category");
Check(ChordCatalog.All.All(t => t.Intervals[0].Semitones == 0), "every formula starts at the root");
foreach (var root in Roots.All)
    foreach (var type in ChordCatalog.All)
    {
        var chord = new Chord(root, type);
        Check(chord.Notes.Select(n => n.PitchClass).Distinct().Count() == chord.Notes.Count, $"{chord.Symbol}: no repeated pitch class");
        for (var inv = 0; inv <= chord.MaxInversion; inv++)
        {
            var v = chord.Voice(inv);
            Check(v.Count == chord.Notes.Count && v.Select(n => n.Midi).Distinct().Count() == v.Count,
                $"{chord.Symbol} inversion {inv}: one key per note");
            Check(v.Min(n => n.Midi) >= 36 && v.Max(n => n.Midi) <= 96, $"{chord.Symbol} inversion {inv}: stays on the keyboard");
        }
    }

// ---------- Roots and URLs ----------
Check(Roots.All.Count == 17, "17 root spellings");
Check(Roots.All.All(r => Roots.FromSlug(r.Slug) == r), "every root round-trips through its URL slug");
Check(Roots.ForPitchClass(1, preferSharps: true).Display == "C♯", "pitch class 1 with sharps is C♯");
Check(Roots.ForPitchClass(1, preferSharps: false).Display == "D♭", "pitch class 1 with flats is D♭");

// ---------- Inversions ----------
var cMaj = new Chord(Roots.C, T("major"));
Check(cMaj.Voice(1).First().Name == "E", "C major 1st inversion has E in the bass");
Check(cMaj.Voice(2).First().Name == "G", "C major 2nd inversion has G in the bass");

// ---------- Search ----------
var s = ChordCatalog.Search("F#m7");
Check(s.Root?.Display == "F♯" && s.Types.FirstOrDefault()?.Id == "m7", "search F#m7");
s = ChordCatalog.Search("Bbmaj7");
Check(s.Root?.Display == "B♭" && s.Types.FirstOrDefault()?.Id == "maj7", "search Bbmaj7");
s = ChordCatalog.Search("dominant");
Check(s.Root is null && s.Types.Any(t => t.Id == "7"), "search dominant: no root, finds the dominant 7th");
s = ChordCatalog.Search("half-dim");
Check(s.Types.Any(t => t.Id == "m7b5"), "search half-dim");
s = ChordCatalog.Search("dim");
Check(s.Root is null && s.Types.FirstOrDefault()?.Id == "dim", "search dim is not D + 'im'");
s = ChordCatalog.Search("sus");
Check(s.Types.Any(t => t.Id == "sus2") && s.Types.Any(t => t.Id == "sus4"), "search sus");
s = ChordCatalog.Search("CM7");
Check(s.Types.FirstOrDefault()?.Id == "maj7", "CM7 is major 7th, not minor 7th");
s = ChordCatalog.Search("Cm7");
Check(s.Types.FirstOrDefault()?.Id == "m7", "Cm7 is minor 7th");
s = ChordCatalog.Search("A minor");
Check(s.Root?.Display == "A" && s.Types.Any(t => t.Id == "minor"), "search A minor");

// ---------- Identify ----------
var m = ChordFinder.Identify([60, 64, 67]);
Check(m.FirstOrDefault()?.Symbol == "C", "C E G is C");
m = ChordFinder.Identify([64, 67, 72]);
Check(m.FirstOrDefault()?.Symbol == "C/E", "E G C is C/E");
m = ChordFinder.Identify([57, 60, 64, 67]);
Check(m.FirstOrDefault()?.Symbol == "Am7" && m.Any(x => x.Symbol == "C6/A"), "A C E G is Am7, also C6/A");
m = ChordFinder.Identify([61, 65, 68], preferSharps: true);
Check(m.FirstOrDefault()?.Symbol == "C♯", "C♯ F G♯ with sharps is C♯");
m = ChordFinder.Identify([61, 65, 68], preferSharps: false);
Check(m.FirstOrDefault()?.Symbol == "D♭", "D♭ F A♭ with flats is D♭");
Check(ChordFinder.Identify([60]).Count == 0, "a single note is not a chord");
Check(ChordFinder.Identify([60, 61, 62]).Count == 0, "a cluster matches nothing");

Console.WriteLine($"{checks - failures}/{checks} checks passed");
return failures;
