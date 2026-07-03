// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

namespace AwaitGodot.Engineering.Maintainer;

public interface IMaintainTask
{
    Task Maintain(Maintaining maintaining);
}
