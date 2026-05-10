// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Collections.Immutable;
using Serilog;

namespace Project.Maintainer.Tasks;

public sealed class FileSyncMaintainTask(ILogger logger,
                                       IEnumerable<FileSystemSync> fileSystemSyncs,
                                       GitService git)
    : FileBasedTask(git)
{
    public ImmutableArray<FileSystemSync> FileSystemSyncs { get; } = [.. fileSystemSyncs];

    public ILogger Logger { get; } = logger.ForContext<FileSystemSync>();

    private void Overlay(string path, string root, Overlay _)
    {
        string upstream = Path.Combine(GitService.UpstreamGitRepoPath, path);
        string local = Path.Combine(root, path);

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

    private async Task OverlayPart(string path, string root, OverlayPart part)
    {
        string upstream = Path.Combine(GitService.UpstreamGitRepoPath, path);
        string local = Path.Combine(root, path);

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

    protected override string[] PrepareFiles(Maintaining maintaining)
    {
        return FileSystemSyncs.Select(a => a.Path).ToArray();
    }

    protected override async Task Execute(Maintaining maintaining)
    {
        var root = maintaining.ProjectRoot;

        foreach (FileSystemSync fileSystemSync in FileSystemSyncs)
        {
            fileSystemSync.Deconstruct(out string path, out SyncPolicy policy);
            switch (policy)
            {
                case Overlay overlay:
                    Overlay(path, root, overlay);
                    break;
                case OverlayPart overlayPart:
                    await OverlayPart(path, root, overlayPart);
                    break;
                default:
                    throw new InvalidOperationException($"unknown {fileSystemSync.GetType().FullName}");
            }
        }
    }
}
