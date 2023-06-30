// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.Spectre;
using Spectre.Console.Cli;

namespace Microsoft.DotNet.Tools.Scaffold.Commands.Commands
{
    internal class ControllerCommand : AsyncCommand<CommandSettings>
    {
        public override Task<int> ExecuteAsync(CommandContext context, CommandSettings settings)
        {
            throw new NotImplementedException();
        }

        public class Settings : DefaultCommandSettings
        {
            [CommandOption("--name")]
            public string Name { get; set; } = default!;
        }
    }
}
