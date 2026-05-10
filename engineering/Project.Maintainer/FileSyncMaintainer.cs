// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Collections.Immutable;
using System.Diagnostics;
using Serilog;

namespace Project.Maintainer;

public sealed class FileSyncMaintainer(ILogger logger,
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
        Git.ShallowCheckout(path);

        if (File.Exists(upstream))
        {
            Logger.Verbose("copy file from {src} to {dst}", upstream, local);
            File.Delete(local);
            File.Copy(upstream, local);
        }

        if (!Directory.Exists(upstream))
        {
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
        await Git.ShallowCheckout(paths);

        if (!File.Exists(upstream))
        {
            File.Delete(local);
            File.Copy(upstream, local);
            Logger.Verbose("copy file from {src} to {dst}", upstream, local);
        }

        await File.WriteAllTextAsync(local,
                                     part.Replace(await File.ReadAllTextAsync(local),
                                                  part.Extract(await File.ReadAllTextAsync(upstream))));
    }

    public async Task Maintain(Maintaining maintaining)
    {
        foreach (FileSystemSync fileSystemSync in FileSystemSyncs)
        {
            switch (fileSystemSync.Policy)
            {
                case Overlay _:
                    Overlay(fileSystemSync.Path, maintaining);
                    break;
                case OverlayPart overlayPart:
                    await OverlayPart(fileSystemSync.Path, overlayPart, maintaining);
                    break;
                default:
                    throw new InvalidOperationException($"unknown {fileSystemSync.GetType().FullName}");
            }
        }
    }
}
