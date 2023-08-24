// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using Microsoft.DotNet.Tools.Scaffold.Commands;
using Spectre.Console.Flow;

namespace Microsoft.DotNet.Tools.Scaffold.Flow.Discoveries
{
    internal class RazorPageDiscovery
    {
        internal RazorPageDiscovery() {}

        internal FlowStepState State { get; private set; }

        internal string? Prompt(IFlowContext context, string title)
        {
            var prompt = new FlowSelectionPrompt<string>()
                .Title(title)
                .AddChoices(RazorPageScaffolders.Keys.ToList(), navigation: context.Navigation);

            var result = prompt.Show();
            State = result.State;
            if (!string.IsNullOrEmpty(result.Value))
            {
                return RazorPageScaffolders.GetValueOrDefault(result.Value);
            }

            return null;
        }

        internal string? Discover(IFlowContext context)
        {
            return Prompt(context, "Which Razor Page scaffolder do you want to use?");
        }

        internal Dictionary<string, string>? _razorPageScaffolders;
        internal Dictionary<string, string> RazorPageScaffolders => _razorPageScaffolders ??=
            new Dictionary<string, string>()
            {
                { "Razor Pages - Empty", "Razor Pages - Empty" }
            };
    }
}

