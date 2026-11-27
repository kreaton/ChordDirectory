namespace ChordDirectory.Music;

/// <summary>A spelled chord tone (name and role), independent of octave.</summary>
public sealed record ChordNote(string Name, string Degree, int Semitones, int PitchClass, bool IsRoot);

/// <summary>A chord tone placed on the keyboard at a specific MIDI note number.</summary>
public sealed record VoicedNote(int Midi, string Name, string Degree, bool IsRoot);

/// <summary>A root + chord type, with correctly spelled notes and keyboard voicings.</summary>
public sealed class Chord
{
    /// <summary>MIDI number of C3: root-position chords are voiced with the root in the octave above this.</summary>
    private const int BaseMidi = 48;

    public Chord(Root root, ChordType type)
    {
        Root = root;
        Type = type;
        Notes = type.Intervals
            .Select(i => new ChordNote(
                Notation.Spell(root, i.Degree, i.Semitones),
                i.Label,
                i.Semitones,
                Notation.Mod12(root.PitchClass + i.Semitones),
                i.Semitones == 0))
            .ToArray();
    }

    public Root Root { get; }
    public ChordType Type { get; }
    public IReadOnlyList<ChordNote> Notes { get; }

    public string Symbol => Root.Display + Type.Symbol;
    public string FullName => $"{Root.Display} {Type.Name}";
    public string Slug => $"{Root.Slug}/{Type.Id}";
    public string Href => $"chord/{Slug}";

    /// <summary>Pitch classes (0–11) in the chord.</summary>
    public IEnumerable<int> PitchClasses => Notes.Select(n => n.PitchClass);

    /// <summary>The number of inversions offered in the UI (root position counts as 0).</summary>
    public int MaxInversion => Math.Min(Notes.Count - 1, 3);

    public static string InversionName(int inversion) => inversion switch
    {
        0 => "Root position",
        1 => "1st inversion",
        2 => "2nd inversion",
        3 => "3rd inversion",
        _ => $"{inversion}th inversion",
    };

    /// <summary>
    /// Returns the chord laid out on the keyboard. Each inversion moves the lowest note up an octave;
    /// voicings that would run too high are dropped an octave so the keyboard stays compact.
    /// </summary>
    public IReadOnlyList<VoicedNote> Voice(int inversion = 0)
    {
        inversion = Math.Clamp(inversion, 0, Notes.Count - 1);
        var baseRoot = BaseMidi + Root.PitchClass;

        var voiced = Notes
            .Select(n => new VoicedNote(baseRoot + n.Semitones, n.Name, n.Degree, n.IsRoot))
            .OrderBy(v => v.Midi)
            .ToList();

        for (var i = 0; i < inversion; i++)
        {
            var lowest = voiced[0];
            voiced.RemoveAt(0);
            voiced.Add(lowest with { Midi = lowest.Midi + 12 });
            voiced = voiced.OrderBy(v => v.Midi).ToList(); // a 9th/11th can sit above the raised note
        }

        while (voiced.Max(v => v.Midi) > 83 && voiced.Min(v => v.Midi) - 12 >= 36)
            voiced = voiced.Select(v => v with { Midi = v.Midi - 12 }).ToList();

        return voiced;
    }

    public override string ToString() => $"{Symbol} ({string.Join(' ', Notes.Select(n => n.Name))})";
}
