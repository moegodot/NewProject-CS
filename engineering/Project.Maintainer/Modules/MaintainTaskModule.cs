// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;
using Project.Maintainer.Tasks;

namespace Project.Maintainer.Modules;

public class MaintainTaskModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileSyncMaintainTask>()
               .SingleInstance()
               .As<IMaintainTask>();
        builder.RegisterType<FileContentExistsCheckTask>()
               .SingleInstance()
               .As<IMaintainTask>();
    }
}
