// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Collections.Concurrent;
using Serilog;

namespace Project.Maintainer;

public class Maintaining
{
    public ILogger Logger { get; }

    public string ProjectRoot { get; }

    public ConcurrentDictionary<string, object?> Items { get; } = [];

    public const string RootMarker = ".await.godot.project.root";

    public Maintaining(ILogger logger)
    {
        Logger = logger.ForContext<Maintaining>();

        string current = Directory.GetCurrentDirectory();
        string file = Path.Combine(current, RootMarker);
        while (!File.Exists(file))
        {
            current = Directory.GetParent(current)?.FullName ?? throw new InvalidOperationException($"failed to find {RootMarker}");
            file = Path.Combine(current, RootMarker);
        }

        var _ = async () => await File.AppendAllTextAsync("", "");

        ProjectRoot = current;
        Logger.Information("Locates project root path {project_root}", ProjectRoot);
    }
}
