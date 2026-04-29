// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Project.Tests;

namespace Project.Maintainer;

public abstract class Maintenance
{
    public abstract string Paths();

    public const string StartColor = "\e[32;45m";
    public const string StopColor = "\e[0m";

    public void Maintain()
    {
        var paths = Paths();
        var src = Path.Combine(Program.UpdateContentDirectory, paths);
        var dst = Path.Combine(Program.RepoDirectory, paths);
        Helper.ShallowCheckout(Program.UpdateContentDirectory, paths);

        if (File.Exists(src))
        {
            File.Delete(dst);
            File.Copy(src,dst);
            Console.WriteLine($"{StartColor}copy file from {src} to {dst}{StopColor}");
        }

        if (!Directory.Exists(src))
        {
            return;
        }

        Directory.Delete(dst);
        Directory.Move(src,
                       dst);
        Console.WriteLine($"{StartColor}copy directory from {src} to {dst}{StopColor}");
    }
}
