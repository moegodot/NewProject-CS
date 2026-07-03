// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

namespace AwaitGodot.Engineering.Maintainer;

public class FileBasedTask(GitService gitService) : IMaintainTask
{
    public GitService GitService { get; } = gitService;

    public async Task Maintain(Maintaining maintaining)
    {
        ArgumentNullException.ThrowIfNull(maintaining);

        var root = maintaining.ProjectRoot;

        var value = (HashSet<string>)
            (maintaining.Items.GetOrAdd(nameof(FileBasedTask), new HashSet<string>())
             ?? throw new InvalidOperationException("expect a nonnull HashSet<String> but got null"));

        var files = PrepareFiles(maintaining)
                    .Distinct()
                    .Select(s => (s, Path.Combine(root, s)))
                    .Where(s => !value.Contains(s.Item2))
                    .ToArray();

        await GitService.ShallowCheckout([.. files.Select(s => s.s)]);

        foreach (var file in files)
        {
            value.Add(file.Item2);
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
