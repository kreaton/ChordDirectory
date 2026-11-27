namespace ChordDirectory.Music;

/// <summary>A chord root with a specific spelling (C♯ and D♭ share a pitch class but are different roots).</summary>
public sealed record Root(int LetterIndex, int Accidental)
{
    public int PitchClass => Notation.Mod12(Notation.NaturalPitchClass[LetterIndex] + Accidental);

    public string Display => Notation.Letters[LetterIndex] + Notation.AccidentalText(Accidental);

    /// <summary>URL-safe identifier: C, Cs (C♯), Db (D♭), ...</summary>
    public string Slug => Notation.Letters[LetterIndex] + Accidental switch { 1 => "s", -1 => "b", _ => "" };

    public override string ToString() => Display;
}

public static class Roots
{
    private static readonly (int Letter, int Acc)[] Defs =
    [
        (0, 0), (0, 1), (1, -1), (1, 0), (1, 1), (2, -1), (2, 0), (3, 0), (3, 1),
        (4, -1), (4, 0), (4, 1), (5, -1), (5, 0), (5, 1), (6, -1), (6, 0),
    ];

    /// <summary>All 17 supported root spellings, ordered chromatically.</summary>
    public static IReadOnlyList<Root> All { get; } = Defs.Select(d => new Root(d.Letter, d.Acc)).ToArray();

    public static Root C { get; } = All[0];

    public static Root? FromSlug(string? slug) =>
        string.IsNullOrEmpty(slug) ? null : All.FirstOrDefault(r => r.Slug.Equals(slug, StringComparison.Ordinal));

    public static Root? FromName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var n = name.Trim().Replace('♯', '#').Replace('♭', 'b');
        return All.FirstOrDefault(r => r.Display.Replace('♯', '#').Replace('♭', 'b')
            .Equals(n, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>The conventional spelling for a pitch class (naturals, then ♯ or ♭ for black keys).</summary>
    public static Root ForPitchClass(int pc, bool preferSharps)
    {
        pc = Notation.Mod12(pc);
        var matches = All.Where(r => r.PitchClass == pc).ToList();
        var natural = matches.FirstOrDefault(r => r.Accidental == 0);
        if (natural is not null) return natural;
        return matches.First(r => r.Accidental == (preferSharps ? 1 : -1));
    }
}

public static class Notation
{
    public static readonly char[] Letters = ['C', 'D', 'E', 'F', 'G', 'A', 'B'];
    public static readonly int[] NaturalPitchClass = [0, 2, 4, 5, 7, 9, 11];

    private static readonly string[] SharpNames = ["C", "C♯", "D", "D♯", "E", "F", "F♯", "G", "G♯", "A", "A♯", "B"];
    private static readonly string[] FlatNames = ["C", "D♭", "D", "E♭", "E", "F", "G♭", "G", "A♭", "A", "B♭", "B"];

    public static int Mod12(int v) => ((v % 12) + 12) % 12;

    public static string AccidentalText(int acc) => acc switch
    {
        -2 => "♭♭",
        -1 => "♭",
        0 => "",
        1 => "♯",
        2 => "x",
        _ => "",
    };

    public static string PitchClassName(int pc, bool preferSharps) =>
        (preferSharps ? SharpNames : FlatNames)[Mod12(pc)];

    public static bool IsBlackKey(int midi) => Mod12(midi) is 1 or 3 or 6 or 8 or 10;

    public static string MidiName(int midi, bool preferSharps = true) =>
        PitchClassName(midi, preferSharps) + (midi / 12 - 1);

    /// <summary>
    /// Spells the note that lies <paramref name="semitones"/> above <paramref name="root"/> and is the
    /// <paramref name="degree"/>-th scale degree (so a major third above D♭ is F, never E♯).
    /// </summary>
    public static string Spell(Root root, int degree, int semitones)
    {
        var letterIndex = (root.LetterIndex + degree - 1) % 7;
        var target = Mod12(root.PitchClass + semitones);
        var diff = Mod12(target - NaturalPitchClass[letterIndex]);
        if (diff > 6) diff -= 12;
        if (diff is < -2 or > 2)
        {
            // Pathological spelling (triple accidentals): fall back to a plain pitch-class name.
            return PitchClassName(target, preferSharps: root.Accidental >= 0);
        }

        return Letters[letterIndex] + AccidentalText(diff);
    }
}
