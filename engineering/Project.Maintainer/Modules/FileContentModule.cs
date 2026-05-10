// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;

namespace Project.Maintainer.Modules;

public class FileContentModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        FileContentExistence[] items =
        [
            new("global.json",
                """
                "sdk": {
                  "version": "10.0.100",
                  "rollForward": "latestFeature",
                  "allowPrerelease": true
                },
                "test": {
                  "runner": "Microsoft.Testing.Platform"
                }
                """.Trim()),

            new("dotnet-tools.json",
                """
                "version": 1,
                "isRoot": true,
                "tools": {
                  "nuget-license": {
                    "version": "4.0.10",
                    "commands": [
                      "nuget-license"
                    ],
                    "rollForward": false
                  }
                }
                """.Trim()),
            new("Directory.Build.props", """<Import Project="$(MSBuildThisFileDirectory)/Project.props" />"""),
            new ("Directory.Build.targets", """<Import Project="$(MSBuildThisFileDirectory)/Project.targets" />"""),
            new ("Directory.Packages.targets", """<Import Project="$(MSBuildThisFileDirectory)/Packages.targets" />"""),
        ];

        foreach (FileContentExistence item in items)
        {
            builder.RegisterInstance(item).AsSelf().SingleInstance();
        }
    }
}
