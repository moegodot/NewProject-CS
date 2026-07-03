// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Serilog;

namespace AwaitGodot.Engineering.Maintainer.Tasks;

public class FileContentExistsCheckTask(ILogger logger,
                                        IEnumerable<FileContentExistence> fileContentExistences,
                                        GitService git)
    : FileBasedTask(git)
{
    public ILogger Logger { get; } = logger;

    protected override string[] PrepareFiles(Maintaining maintaining)
    {
        return fileContentExistences.Select(s => s.Path).ToArray();
    }

    protected override async Task Execute(Maintaining maintaining)
    {
        var root = maintaining.ProjectRoot;
        foreach (FileContentExistence contentExistence in fileContentExistences)
        {
            Logger.Verbose("check {file} content", contentExistence.Path);
            if (!(await File.ReadAllTextAsync(Path.Combine(root, contentExistence.Path)))
               .Contains(contentExistence.Content))
            {
                throw new InvalidOperationException($"file {contentExistence.Path} do not contains:\n{contentExistence.Content}");
            }
        }
    }
}
