// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Microsoft.DotNet.Tools.Scaffold.Commands.Commands
{
    internal class AreaCommand : Command<CommandSettings>
    {
        public class Settings : ScaffolderSettings
        {
            [CommandOption("--name")]
            public string Name { get; set; } = default!;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] CommandSettings settings)
        {
            //settings.ValidateScaffolderSettings();
            ValidateAreaArgs(settings);

            return EnsureFolderLayout(settings);
        }

        private void ValidateAreaArgs(CommandSettings settings)
        {
            bool validAreaName = false;
            while (!validAreaName)
            {
                string areaName = AnsiConsole.Ask<string>("\nProvide an area name:");
                if (!string.IsNullOrEmpty(areaName))
                {
                    validAreaName = true;
                    //settings.Name = areaName;
                }
            }
        }

        private int EnsureFolderLayout(CommandSettings settings)
        {
            string? applicationBasePath = string.Empty;// Path.GetDirectoryName(settings.ProjectPath);
            if (string.IsNullOrWhiteSpace(applicationBasePath) || !Directory.Exists(applicationBasePath))
            {
                return -1;
            }

            string? areaBasePath = Path.Combine(applicationBasePath, "Areas");
            if (!Directory.Exists(areaBasePath))
            {
                Directory.CreateDirectory(areaBasePath);
            }

            var areaPath = string.Empty;// Path.Combine(areaBasePath, settings.Name);
            if (!Directory.Exists(areaPath))
            {
                Directory.CreateDirectory(areaPath);
            }

            foreach (var areaFolder in AreaFolders)
            {
                var path = Path.Combine(areaPath, areaFolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            return 0;
        }

        private static readonly string[] AreaFolders = new string[]
        {
            "Controllers",
            "Models",
            "Data",
            "Views"
        };
    }
}
