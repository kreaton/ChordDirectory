namespace ChordDirectory.Music;

public sealed record SearchResult(Root? Root, IReadOnlyList<ChordType> Types);

public static class ChordCatalog
{
    public const string Triads = "Triads";
    public const string Sixths = "Sixths";
    public const string Sevenths = "Sevenths";
    public const string Ninths = "Ninths";
    public const string Extended = "11ths & 13ths";
    public const string Altered = "Altered & Suspended";

    public static IReadOnlyList<string> Categories { get; } =
        [Triads, Sixths, Sevenths, Ninths, Extended, Altered];

    public static IReadOnlyList<ChordType> All { get; } =
    [
        // ── Triads ───────────────────────────────────────────────
        new("major", "Major", "", Triads, "1 3 5",
            "Bright and stable. The foundation of most Western harmony.", "maj", "M"),
        new("minor", "Minor", "m", Triads, "1 b3 5",
            "Darker and more introspective than major.", "min", "-"),
        new("dim", "Diminished", "dim", Triads, "1 b3 b5",
            "Tense and unstable, wants to resolve. Two stacked minor thirds.", "°", "mb5"),
        new("aug", "Augmented", "aug", Triads, "1 3 #5",
            "Dreamy and unresolved. Two stacked major thirds.", "+", "#5"),
        new("sus2", "Suspended 2nd", "sus2", Triads, "1 2 5",
            "Open and airy: the third is replaced by the 2nd.", "2"),
        new("sus4", "Suspended 4th", "sus4", Triads, "1 4 5",
            "Hangs in the air until the 4th resolves down to the 3rd.", "sus"),
        new("power", "Power chord", "5", Triads, "1 5",
            "Just root and fifth. Neither major nor minor.", "~power"),

        // ── Sixths ───────────────────────────────────────────────
        new("6", "Major 6th", "6", Sixths, "1 3 5 6",
            "A sweet, jazzy major chord with an added major sixth.", "maj6", "M6"),
        new("m6", "Minor 6th", "m6", Sixths, "1 b3 5 6",
            "Minor triad with a major sixth: wistful and slightly bittersweet.", "min6", "-6"),
        new("6-9", "Six-nine", "6/9", Sixths, "1 3 5 6 9",
            "A lush, resolved ending chord, common in jazz and pop.", "69", "6add9"),

        // ── Sevenths ─────────────────────────────────────────────
        new("maj7", "Major 7th", "maj7", Sevenths, "1 3 5 7",
            "Smooth and dreamy: a major triad with a major seventh.", "M7", "Δ", "Δ7", "~major7"),
        new("7", "Dominant 7th", "7", Sevenths, "1 3 5 b7",
            "The bluesy, driving chord that pulls toward the tonic.", "dom7", "~dominant"),
        new("m7", "Minor 7th", "m7", Sevenths, "1 b3 5 b7",
            "Mellow and soulful. A staple of jazz, R&B and pop.", "min7", "-7"),
        new("mmaj7", "Minor-major 7th", "m(maj7)", Sevenths, "1 b3 5 7",
            "Dark and cinematic: a minor triad with a major seventh.", "mM7", "m#7", "minmaj7", "-Δ7"),
        new("dim7", "Diminished 7th", "dim7", Sevenths, "1 b3 b5 bb7",
            "Symmetrical and dramatic: a stack of minor thirds.", "°7", "o7"),
        new("m7b5", "Half-diminished", "m7♭5", Sevenths, "1 b3 b5 b7",
            "The ii chord in minor keys. Also written ø.", "m7b5", "ø", "ø7", "~half-dim", "~halfdim"),
        new("7sus4", "Dominant 7 sus4", "7sus4", Sevenths, "1 4 5 b7",
            "Suspended and open, with the bluesy flat seventh.", "7sus"),
        new("aug7", "Augmented 7th", "7♯5", Sevenths, "1 3 #5 b7",
            "Dominant 7th with a raised fifth: tense and colourful.", "7#5", "aug7", "+7"),
        new("maj7s5", "Major 7th ♯5", "maj7♯5", Sevenths, "1 3 #5 7",
            "Floating and otherworldly: augmented triad plus a major seventh.", "maj7#5", "M7#5"),
        new("7b5", "Dominant 7th ♭5", "7♭5", Sevenths, "1 3 b5 b7",
            "Dominant 7th with a lowered fifth: tense, with a whole-tone flavour.", "7b5"),

        // ── Ninths ───────────────────────────────────────────────
        new("add9", "Added 9th", "add9", Ninths, "1 3 5 9",
            "Major triad plus a 9th with no seventh: bright and modern.", "add2"),
        new("madd9", "Minor added 9th", "m(add9)", Ninths, "1 b3 5 9",
            "Minor triad with an added 9th. Moody and spacious.", "madd9", "~m add9"),
        new("maj9", "Major 9th", "maj9", Ninths, "1 3 5 7 9",
            "Rich and glossy: major 7th with a 9th on top.", "M9"),
        new("9", "Dominant 9th", "9", Ninths, "1 3 5 b7 9",
            "Funk and soul favourite: a dominant 7th with a 9th.", "dom9"),
        new("m9", "Minor 9th", "m9", Ninths, "1 b3 5 b7 9",
            "Smooth and sophisticated minor colour.", "min9", "-9"),
        new("7b9", "Dominant 7th ♭9", "7♭9", Ninths, "1 3 5 b7 b9",
            "Spicy, tense dominant often used before a minor chord.", "7b9"),
        new("7s9", "Dominant 7th ♯9", "7♯9", Ninths, "1 3 5 b7 #9",
            "The \"Hendrix chord\": gritty, bluesy and bold.", "7#9", "~hendrix"),
        new("9sus4", "Dominant 9 sus4", "9sus4", Ninths, "1 4 5 b7 9",
            "Open, suspended dominant with a 9th.", "9sus"),

        // ── 11ths & 13ths ────────────────────────────────────────
        new("11", "Dominant 11th", "11", Extended, "1 3 5 b7 9 11",
            "Big stacked dominant. The 11th often clashes with the 3rd, so it's often voiced without it.", "dom11"),
        new("m11", "Minor 11th", "m11", Extended, "1 b3 5 b7 9 11",
            "Deep, atmospheric minor chord.", "min11", "-11"),
        new("maj7s11", "Major 7th ♯11", "maj7♯11", Extended, "1 3 5 7 #11",
            "The Lydian chord: shimmering and bright.", "maj7#11", "M7#11", "~lydian"),
        new("7s11", "Dominant 7th ♯11", "7♯11", Extended, "1 3 5 b7 #11",
            "Lydian-dominant colour: jazzy and slightly unsettled.", "7#11"),
        new("13", "Dominant 13th", "13", Extended, "1 3 5 b7 9 13",
            "The full jazz dominant: rich, big-band sound.", "dom13"),
        new("maj13", "Major 13th", "maj13", Extended, "1 3 5 7 9 13",
            "Lush and expansive major colour.", "M13"),
        new("m13", "Minor 13th", "m13", Extended, "1 b3 5 b7 9 13",
            "Smooth, sophisticated minor with a bright sixth/13th on top.", "min13", "-13"),

        // ── Altered & Suspended ──────────────────────────────────
        new("7b13", "Dominant 7th ♭13", "7♭13", Altered, "1 3 5 b7 b13",
            "A dominant with a minor-flavoured flat 13th.", "7b13", "7b6"),
        new("7alt", "Altered dominant", "7alt", Altered, "1 3 b7 #9 b13",
            "A dominant loaded with altered tensions (♯9 and ♭13): maximum pull to resolve.", "alt"),
        new("add11", "Added 11th", "add11", Altered, "1 3 5 11",
            "Major triad with an 11th on top. A bright, open colour.", "add4"),
    ];

    private static readonly Dictionary<string, ChordType> ById = All.ToDictionary(c => c.Id);

    public static ChordType? Find(string? id) =>
        id is not null && ById.TryGetValue(id, out var t) ? t : null;

    public static IEnumerable<ChordType> InCategory(string category) => All.Where(c => c.Category == category);

    /// <summary>
    /// Free-text search: "F#m7", "dominant", "maj7", "sus", "Bb". A leading note name selects the root.
    /// </summary>
    public static SearchResult Search(string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new SearchResult(null, All);

        var q = query.Trim().Replace('♯', '#').Replace('♭', 'b');

        // Try to read a root off the front of the query ("Bb7", "F#", "a").
        Root? root = null;
        var rest = q;
        if (q[0] is (>= 'A' and <= 'G') or (>= 'a' and <= 'g'))
        {
            if (q.Length > 1 && q[1] is '#' or 'b')
            {
                var withAcc = Roots.FromName(q[..2]);
                if (withAcc is not null)
                {
                    root = withAcc;
                    rest = q[2..];
                }
            }

            if (root is null)
            {
                // "bass" or "a minor" start with a note letter but aren't roots: only accept when the
                // remainder is empty, starts with a symbol/space, or is a known chord suffix.
                var candidate = Roots.FromName(q[..1]);
                var tail = q[1..];
                if (candidate is not null && (tail.Length == 0 || !char.IsLetter(tail[0]) || LooksLikeSuffix(tail)))
                {
                    root = candidate;
                    rest = tail;
                }
            }
        }

        rest = rest.Trim();
        if (rest.Length == 0) return new SearchResult(root, All);

        var exact = All.Where(t => t.Symbol.Replace('♯', '#').Replace('♭', 'b') == rest
                                   || t.SearchTerms.Contains(rest)).ToList();

        var loose = All.Where(t => !exact.Contains(t) && (
            t.Name.Contains(rest, StringComparison.OrdinalIgnoreCase)
            || t.Id.StartsWith(rest, StringComparison.OrdinalIgnoreCase)
            || t.Symbol.Replace('♯', '#').Replace('♭', 'b').StartsWith(rest, StringComparison.OrdinalIgnoreCase)
            || t.SearchTerms.Any(a => a.StartsWith(rest, StringComparison.OrdinalIgnoreCase)))).ToList();

        var types = exact.Concat(loose).ToList();

        // Nothing matched after parsing a root? Maybe the whole thing was a name ("bass" etc.).
        if (types.Count == 0 && root is not null)
        {
            var whole = All.Where(t => t.Name.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            if (whole.Count > 0) return new SearchResult(null, whole);
        }

        return new SearchResult(root, types);
    }

    private static bool LooksLikeSuffix(string tail) =>
        All.Any(t => t.Symbol.Replace('♯', '#').Replace('♭', 'b').StartsWith(tail, StringComparison.Ordinal)
                     || t.SearchTerms.Any(a => a.StartsWith(tail, StringComparison.Ordinal)));
}
