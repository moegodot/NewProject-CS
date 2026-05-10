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
        return Helper.Git(Logger,
                          Directory.GetParent(UpstreamGitRepoPath)!.FullName,
                          "clone",
                          "--depth=1",
                          "--filter=blob:none",
                          "--sparse",
                          Upstream,
                          Path.GetFileName(UpstreamGitRepoPath));
    }

    public Task ShallowCheckout(string fileOrDir)
    {
        return Helper.Git(Logger,
                   UpstreamGitRepoPath,
                   "sparse-checkout",
                   "set",
                   "--skip-checks",
                   fileOrDir);
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

        ShallowClone().GetAwaiter().GetResult();
        Update().GetAwaiter().GetResult();
    }
}
