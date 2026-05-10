// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;

namespace Project.Maintainer;

public class MaintainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileSyncMaintainTask>()
               .SingleInstance()
               .As<IMaintainTask>();
    }
}
