// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Project.Workspaces;

namespace Microsoft.DotNet.Scaffolding.Shared.Project
{
    public static class ModelTypesHelper
    {
        public static IEnumerable<ModelType> GetMatchingTypes(this RoslynWorkspace projectWorkspace, string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            var exactTypesInAllProjects = projectWorkspace
                .CurrentSolution.Projects
                .Select(project => project.GetCompilationAsync().Result)
                .Select(comp => comp.Assembly.GetTypeByMetadataName(typeName) as ITypeSymbol)
                .Where(type => type != null);

            if (exactTypesInAllProjects.Any())
            {
                return exactTypesInAllProjects.Select(ts => ModelType.FromITypeSymbol(ts));
            }
            //For short type names, we don't give special preference to types in current app,
            //should we do that?
            return projectWorkspace.GetAllTypes()
                .Where(type => string.Equals(type.Name, typeName, StringComparison.Ordinal));
        }

        internal static IEnumerable<ModelType> GetAllTypes(this RoslynWorkspace projectWorkspace, ExistingClassProperties existingClassProperties = null)
        {
            var allTypes = projectWorkspace.CurrentSolution.Projects
                .Select(project => project.GetCompilationAsync().Result)
                .Select(comp => RoslynUtilities.GetDirectTypesInCompilation(comp))
                .Aggregate((col1, col2) => col1.Concat(col2).ToList())
                .Select(ts => ModelType.FromITypeSymbol(ts));

            if (existingClassProperties is null)
            {
                return allTypes;
            }

            if (!string.IsNullOrEmpty(existingClassProperties.BaseClass))
            {
                int lastDotIndex = existingClassProperties.BaseClass.LastIndexOf('.');
                string shortTypeName = existingClassProperties.BaseClass.Substring(lastDotIndex + 1);
                allTypes = allTypes.Where(x =>
                    x.BaseType.ToDisplayParts().Equals(existingClassProperties.BaseClass) ||
                    x.BaseType.ToDisplayParts().Equals(shortTypeName));
            }

            if (existingClassProperties.IsStatic)
            {
                allTypes = allTypes.Where(x => x.TypeSymbol.IsStatic);
            }

            return allTypes;
        }
    }

    internal class ExistingClassProperties
    {
        public bool IsStatic { get; set; }
        public string BaseClass { get; set; }
    }

}
