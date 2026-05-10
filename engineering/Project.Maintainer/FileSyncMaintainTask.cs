// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Collections.Immutable;
using System.Diagnostics;
using Serilog;

namespace Project.Maintainer;

public sealed class FileSyncMaintainTask(ILogger logger,
                                       IEnumerable<FileSystemSync> fileSystemSyncs,
                                       GitService git)
    : IMaintainTask
{
    public ImmutableArray<FileSystemSync> FileSystemSyncs { get; } = [.. fileSystemSyncs];

    public ILogger Logger { get; } = logger.ForContext<FileSystemSync>();

    public GitService Git { get; } = git;

    private void Overlay(string path, Maintaining maintaining)
    {
        string upstream = Path.Combine(Git.UpstreamGitRepoPath, path);
        string local = Path.Combine(maintaining.ProjectRoot, path);

        if (File.Exists(upstream))
        {
            Logger.Verbose("copy file from {src} to {dst}", upstream, local);
            File.Delete(local);
            File.Copy(upstream, local);
            return;
        }
        Logger.Verbose("copy directory from {src} to {dst}", upstream, local);

        Directory.Delete(local, true);
        Directory.Move(upstream,
                       local);
    }

    private async Task OverlayPart(string path, OverlayPart part, Maintaining maintaining)
    {
        string paths = path;
        string upstream = Path.Combine(Git.UpstreamGitRepoPath, paths);
        string local = Path.Combine(maintaining.ProjectRoot, paths);

        if (!File.Exists(local))
        {
            Logger.Verbose("copy file from {src} to {dst}", upstream, local);
            File.Copy(upstream, local);
            return;
        }

        Logger.Verbose("update file at {src} from {dst} for {mark}",
                       local,
                       upstream,
                       part.PartStartMark);
        await File.WriteAllTextAsync(local,
                                     part.Replace(await File.ReadAllTextAsync(local),
                                                  part.Extract(await File.ReadAllTextAsync(upstream))));
    }

    public async Task Maintain(Maintaining maintaining)
    {
        List<(Overlay, FileSystemSync)> overlays = [];
        List<(OverlayPart, FileSystemSync)> overlayParts = [];
        List<string> paths = [];

        foreach (FileSystemSync fileSystemSync in FileSystemSyncs)
        {
            paths.Add(fileSystemSync.Path);
            switch (fileSystemSync.Policy)
            {
                case Overlay overlay:
                    overlays.Add((overlay, fileSystemSync));
                    break;
                case OverlayPart overlayPart:
                    overlayParts.Add((overlayPart, fileSystemSync));
                    break;
                default:
                    throw new InvalidOperationException($"unknown {fileSystemSync.GetType().FullName}");
            }
        }

        await Git.ShallowCheckout([.. paths]);

        foreach (var overlay in overlays)
        {
            Overlay(overlay.Item2.Path, maintaining);
        }
        foreach (var overlay in overlayParts)
        {
            await OverlayPart(overlay.Item2.Path, overlay.Item1, maintaining);
        }
    }
}
