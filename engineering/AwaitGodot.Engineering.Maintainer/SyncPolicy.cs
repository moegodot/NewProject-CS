// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

namespace AwaitGodot.Engineering.Maintainer;

public abstract record SyncPolicy;

/// <summary>
/// overlay files/directories
/// </summary>
public record Overlay : SyncPolicy;

/// <summary>
/// overlay a part of a file
/// </summary>
/// <param name="PartStartMark">the start of part that need to be replaced</param>
/// <param name="PartEndMark">the end of part that need to be replaced</param>
public record OverlayPart(string PartStartMark,
                          string PartEndMark)
                : SyncPolicy
{

    private static string Id(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
        return id;
    }

    public static string XmlBegin(string id) =>
        $"<!-- {Id(id)} {Program.UniqueBrandMarker} {Program.UniqueBrandStartMarker} -->";

    public static string XmlEnd(string id) =>
        $"<!-- {Id(id)} {Program.UniqueBrandMarker} {Program.UniqueBrandEndMarker} -->";


    public static string HashBegin(string id) =>
        $"# {Id(id)} {Program.UniqueBrandMarker} {Program.UniqueBrandStartMarker}";

    public static string HashEnd(string id) =>
        $"# {Id(id)} {Program.UniqueBrandMarker} {Program.UniqueBrandEndMarker}";

    public static OverlayPart Xml(string id) =>
        new(XmlBegin(id), XmlEnd(id));

    public static OverlayPart Hash(string id) =>
        new(HashBegin(id), HashEnd(id));

    private static int FindMark(string input, string mark)
    {
        int index = input.IndexOf(mark, StringComparison.InvariantCulture);
        if (index == -1)
        {
            throw new ArgumentException($"the input contains no {mark}", nameof(input));
        }
        if (index != input.LastIndexOf(mark, StringComparison.InvariantCulture))
        {
            throw new ArgumentException($"the input contains {mark} more than once", nameof(input));
        }

        return index;
    }

    public string Extract(string input)
    {
        var start = FindMark(input, PartStartMark);
        var end = FindMark(input, PartEndMark);

        return input[(start + PartStartMark.Length)..end];
    }

    public string Replace(string input, string replaceTo)
    {
        var start = FindMark(input, PartStartMark);
        var end = FindMark(input, PartEndMark);

        return $"{input[..(start + PartStartMark.Length)]}{replaceTo}{input[end..]}";
    }
}
