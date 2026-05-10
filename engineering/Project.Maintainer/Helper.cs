// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Diagnostics;
using Serilog;

namespace Project.Maintainer;

internal static class Helper
{
    public static async Task Git(ILogger logger, string dir, params string[] arguments)
    {
        string exe = OperatingSystem.IsWindows() ? ".exe" : string.Empty;
        logger.Verbose("run {executable} {arguments} at {directory}",
            $"git{exe}",
            arguments,
            dir);
        var info = new ProcessStartInfo
        {
            FileName = $"git{exe}",
            UseShellExecute = false,
            WorkingDirectory = dir,
            CreateNoWindow = true,
        };
        foreach (string arg in arguments)
        {
            info.ArgumentList.Add(arg);
        }
        var process = Process.Start(info) ?? throw new InvalidOperationException($"failed to start git{exe} process");

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"failed to execute git{exe} {string.Join(' ', arguments)} at {dir}");
        }
    }
}
