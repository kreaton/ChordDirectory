namespace ChordDirectory.Music;

/// <summary>A chord that matches a set of keys, with the lowest key shown as a slash-bass when it isn't the root.</summary>
public sealed record ChordMatch(Chord Chord, string? BassName)
{
    public bool IsRootPosition => BassName is null;

    public string Symbol => BassName is null ? Chord.Symbol : $"{Chord.Symbol}/{BassName}";

    public string Name => BassName is null ? Chord.FullName : $"{Chord.FullName} over {BassName}";
}

public static class ChordFinder
{
    /// <summary>
    /// Finds every catalogued chord whose notes exactly equal the pitch classes of <paramref name="midiNotes"/>.
    /// Chords whose root is the lowest note come first; other matches are reported as slash chords.
    /// </summary>
    public static IReadOnlyList<ChordMatch> Identify(IEnumerable<int> midiNotes, bool preferSharps = false)
    {
        var notes = midiNotes.ToList();
        if (notes.Count < 2) return [];

        var mask = notes.Aggregate(0, (m, n) => m | (1 << Notation.Mod12(n)));
        var bassPc = Notation.Mod12(notes.Min());
        var results = new List<(ChordMatch Match, int Order)>();

        for (var rootPc = 0; rootPc < 12; rootPc++)
        {
            foreach (var (type, index) in ChordCatalog.All.Select((t, i) => (t, i)))
            {
                if (Rotate(type.PitchClassMask, rootPc) != mask) continue;

                var chord = new Chord(Roots.ForPitchClass(rootPc, preferSharps), type);
                string? bass = null;
                if (bassPc != rootPc)
                    bass = chord.Notes.First(n => n.PitchClass == bassPc).Name;

                // Root-position matches first, then simpler chords (fewer notes), then catalogue order.
                var order = (bass is null ? 0 : 1000) + type.Intervals.Count * 100 + index;
                results.Add((new ChordMatch(chord, bass), order));
            }
        }

        return results.OrderBy(r => r.Order).Select(r => r.Match).ToList();
    }

    private static int Rotate(int mask, int by) => ((mask << by) | (mask >> (12 - by))) & 0xFFF;
}
