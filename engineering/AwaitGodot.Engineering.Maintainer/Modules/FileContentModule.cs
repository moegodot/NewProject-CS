// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;

namespace AwaitGodot.Engineering.Maintainer.Modules;

public class FileContentModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        FileContentExistence[] items =
        [
            new("global.json",
                """
                "runner": "Microsoft.Testing.Platform"
                """.Trim()),
            new("dotnet-tools.json", "nuget-license"),
            new("Directory.Build.props", """<Import Project="$(MSBuildThisFileDirectory)/Project.props" />"""),
            new ("Directory.Build.targets", """<Import Project="$(MSBuildThisFileDirectory)/Project.targets" />"""),
            new ("Directory.Packages.props", """<Import Project="$(MSBuildThisFileDirectory)/Packages.props" />"""),
        ];

        foreach (FileContentExistence item in items)
        {
            builder.RegisterInstance(item).AsSelf().SingleInstance();
        }
    }
}
