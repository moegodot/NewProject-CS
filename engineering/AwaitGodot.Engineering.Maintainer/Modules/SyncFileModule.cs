// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;

namespace AwaitGodot.Engineering.Maintainer.Modules;

public class SyncFileModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        FileSystemSync[] items =
            [
                new("Project.targets", new Overlay()),
                new("Project.props", new Overlay()),
                new("images/icon.png", new Overlay()),
                new("images/icon.kra", new Overlay()),
                new("images/readme_image_1.png", new Overlay()),
                new("images/contributing_image_1.png", new Overlay()),
                new("Packages.props", new Overlay()),
                new("dprint.json", new Overlay()),
                new("gitleaks.toml", new Overlay()),
                new("commitlint.config.js", new Overlay()),
                new("tsconfig.json", new Overlay()),
                new("global.json", new Overlay()),
                new(".editorconfig", new Overlay()),
                new("allowed-licenses.json", new Overlay()),
                new("lefthook.basic.lints.yml", new Overlay()),
                new("lefthook.yml", OverlayPart.Hash("CI")),
                new("CONTRIBUTING.md", OverlayPart.Xml("TitleImage")),
                new("Directory.Build.props", OverlayPart.Xml("Include")),
                new("Directory.Build.targets", OverlayPart.Xml("Include")),
                new("Directory.Packages.props", OverlayPart.Xml("Packaging")),
            ];

        foreach (FileSystemSync item in items)
        {
            builder.RegisterInstance(item).AsSelf().SingleInstance();
        }
    }
}
