namespace ChordDirectory.Components;

/// <summary>Highlights a piano key and labels it (a note name or interval); the root gets its own colour.</summary>
public sealed record KeyMark(string Label, bool IsRoot = false);
