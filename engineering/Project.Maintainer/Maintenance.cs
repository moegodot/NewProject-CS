// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Project.Tests;

namespace Project.Maintainer;

public abstract class Maintenance
{
    public abstract string Paths();

    public void Maintain()
    {
        var paths = Paths();
        var src = Path.Combine(Program.UpdateContentDirectory, paths);
        var dst = Path.Combine(Program.RepoDirectory, paths);
        Helper.ShallowCheckout(Program.UpdateContentDirectory, paths);
        Directory.Delete(dst);
        Directory.Move(src,
                       dst);
    }
}
