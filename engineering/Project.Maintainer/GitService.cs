// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace Project.Maintainer;

public class GitService
{
    public ILogger Logger { get; }

    public string UpstreamGitRepoPath { get; }

    public const string Upstream = "https://github.com/moegodot/NewProject-CS";

    private Task ShallowClone()
    {
        var directoryInfo = new DirectoryInfo(UpstreamGitRepoPath);
        return Helper.Git(Logger,
                          directoryInfo.Parent?.FullName
                          ?? throw new InvalidOperationException("the target directory have no parent"),
                          "clone",
                          "--depth=1",
                          "--filter=blob:none",
                          "--sparse",
                          Upstream,
                          directoryInfo.Name);
    }

    public Task ShallowCheckout(params string[] fileOrDir)
    {
        if (fileOrDir.Length == 0)
        {
            return Task.CompletedTask;
        }

        return Helper.Git(Logger,
                   UpstreamGitRepoPath,
                   ["sparse-checkout",
                   "set",
                   "--skip-checks",
                   ..fileOrDir
                   ]
                   );
    }

    public Task Update()
    {
        return Helper.Git(Logger,
                   UpstreamGitRepoPath,
                   "pull");
    }

    public GitService(ILogger logger, Maintaining maintaining)
    {
        Logger = logger.ForContext<GitService>();

        string name = $"./{Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(maintaining.ProjectRoot)))}-kwi-godot/";

        UpstreamGitRepoPath = Path.Combine(Path.GetTempPath(), name);

        Directory.CreateDirectory(Directory.GetParent(UpstreamGitRepoPath)!.FullName);

        if (Directory.Exists($"{UpstreamGitRepoPath}/.git")
            && (Directory.GetFileSystemEntries($"{UpstreamGitRepoPath}/.git").Length != 0))
        {
            Update().GetAwaiter().GetResult();
        }
        else
        {
            ShallowClone().GetAwaiter().GetResult();
        }
    }
}
