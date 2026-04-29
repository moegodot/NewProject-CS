// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using System.Diagnostics;

namespace Project.Tests;

internal static class Helper
{
    private static void Git(string dir,params string[] arguments)
    {
        string exe = OperatingSystem.IsWindows() ? ".exe" : string.Empty;
        var info = new ProcessStartInfo
        {
            FileName = $"git{exe}", UseShellExecute = false, WorkingDirectory = dir, CreateNoWindow = true,
        };
        foreach (string arg in arguments)
        {
            info.ArgumentList.Add(arg);
        }
        var process = Process.Start(info) ?? throw new InvalidOperationException($"failed to start git{exe} process");

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"failed to execute git{exe} {string.Join(' ',arguments)} at {dir}");
        }
    }

    public static void ShallowClone(string url,string dir,string to)
    {
        Git(dir, "clone",
            "--depth=1",
            "--filter=blob:none",
            "--sparse",
            url,
            to);
    }

    public static void ShallowCheckout(string dir,string fileOrDir)
    {
        Git(dir,
            "sparse-checkout",
            "set",
            fileOrDir);
    }

    public static void Update(string dir)
    {
        Git(dir,
            "pull");
    }
}
