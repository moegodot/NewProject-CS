// Copyright (c) 2026 GodotAsync<me@kawayi.moe>.
// Licensed under the GNU Affero General Public License v3-or-later license.

using Autofac;

namespace Project.Maintainer;

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
                new("readme_image_1.png", new Overlay()),
                new("contributing_image_1.png", new Overlay()),
                new("Packages.targets", new Overlay()),
                new("CONTRIBUTING.md", OverlayPart.Xml("TitleImage")),
                new("README.md", OverlayPart.Xml("TitleImage")),
            ];

        foreach (FileSystemSync item in items)
        {
            builder.RegisterInstance(item).AsSelf().SingleInstance();
        }
    }
}
