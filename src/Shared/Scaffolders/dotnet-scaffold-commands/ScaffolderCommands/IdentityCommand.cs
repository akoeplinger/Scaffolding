// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.Spectre;
using Spectre.Console.Cli;

namespace Microsoft.DotNet.Tools.Scaffold.Commands
{
    internal class IdentityCommand : AsyncCommand<IdentityCommand.Settings>
    {
        public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
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
