namespace ChordDirectory.Music;

/// <summary>One note of a chord formula, e.g. the flattened third (♭3 = 3 semitones, 3rd scale degree).</summary>
public sealed record Interval(int Degree, int Semitones, string Label)
{
    private static readonly Dictionary<int, int> MajorScaleSemitones = new()
    {
        [1] = 0, [2] = 2, [3] = 4, [4] = 5, [5] = 7, [6] = 9, [7] = 11, [9] = 14, [11] = 17, [13] = 21,
    };

    /// <summary>Parses tokens such as "1", "b3", "#5", "bb7", "#11".</summary>
    public static Interval Parse(string token)
    {
        var acc = 0;
        var i = 0;
        while (i < token.Length && token[i] is 'b' or '#')
        {
            acc += token[i] == 'b' ? -1 : 1;
            i++;
        }

        var degree = int.Parse(token[i..]);
        if (!MajorScaleSemitones.TryGetValue(degree, out var baseSemitones))
            throw new FormatException($"Unsupported degree '{token}'.");

        var label = new string(token.Select(c => c switch { 'b' => '♭', '#' => '♯', _ => c }).ToArray());
        return new Interval(degree, baseSemitones + acc, degree == 1 && acc == 0 ? "R" : label);
    }

    public static IReadOnlyList<Interval> ParseFormula(string formula) =>
        formula.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse).ToArray();
}

public sealed class ChordType
{
    public ChordType(string id, string name, string symbol, string category, string formula,
        string description, params string[] aliases)
    {
        Id = id;
        Name = name;
        Symbol = symbol;
        Category = category;
        Formula = formula;
        Description = description;
        // Aliases prefixed with '~' are plain-language search keywords, not ways to write the chord.
        Aliases = aliases.Where(a => !a.StartsWith('~')).ToArray();
        SearchTerms = aliases.Select(a => a.TrimStart('~')).ToArray();
        Intervals = Interval.ParseFormula(formula).OrderBy(i => i.Semitones).ToArray();
        PitchClassMask = Intervals.Aggregate(0, (m, i) => m | (1 << Notation.Mod12(i.Semitones)));
    }

    public string Id { get; }
    public string Name { get; }

    /// <summary>The suffix that follows the root in a chord symbol ("" for major, "m7", "maj9", ...).</summary>
    public string Symbol { get; }

    public string Category { get; }
    public string Formula { get; }
    public string Description { get; }
    /// <summary>Other ways to write the symbol (e.g. "M7", "Δ").</summary>
    public IReadOnlyList<string> Aliases { get; }

    /// <summary>Aliases plus search keywords such as "dominant" or "lydian".</summary>
    public IReadOnlyList<string> SearchTerms { get; }
    public IReadOnlyList<Interval> Intervals { get; }

    /// <summary>Bit i is set when pitch class i (relative to the root) is part of the chord.</summary>
    public int PitchClassMask { get; }

    /// <summary>Symbol for UI lists, where an empty suffix would otherwise look blank.</summary>
    public string SymbolOrMajor => Symbol.Length == 0 ? "maj" : Symbol;

    /// <summary>The distances between neighbouring chord tones, e.g. "4-3-4".</summary>
    public string StepPattern => string.Join("–",
        Intervals.Zip(Intervals.Skip(1), (a, b) => b.Semitones - a.Semitones));
}
