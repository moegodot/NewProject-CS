// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

namespace Project.Maintainer;

public class FileBasedTask(GitService gitService) : IMaintainTask
{
    public GitService GitService { get; } = GitService;

    public async Task Maintain(Maintaining maintaining)
    {
        var value = (HashSet<string>)
            (maintaining.Items.GetOrAdd(nameof(FileBasedTask), new HashSet<string>())
             ?? throw new InvalidOperationException("expect a nonnull HashSet<String> but got null"));

        var files = PrepareFiles(maintaining).Distinct().Where(s => !value.Contains(s)).ToArray();

        await GitService.ShallowCheckout(files);

        foreach (string file in files)
        {
            value.Add(file);
        }

        await Execute(maintaining);
    }

    protected virtual string[] PrepareFiles(Maintaining maintaining)
    {
        return [];
    }

    protected virtual Task Execute(Maintaining maintaining)
    {
        return Task.CompletedTask;
    }
}
