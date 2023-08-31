// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.DotNet.Scaffolding.Shared.Project
{
    internal static class RoslynUtilities
    {
        public static IEnumerable<ITypeSymbol> GetDirectTypesInCompilation(CodeAnalysis.Compilation compilation)
        {
            if (compilation == null)
            {
                throw new ArgumentNullException(nameof(compilation));
            }
            var types = new List<ITypeSymbol>();
            CollectTypes(compilation.Assembly.GlobalNamespace, types);
            return types;
        }

        private static void CollectTypes(INamespaceSymbol ns, List<ITypeSymbol> types)
        {
            types.AddRange(ns.GetTypeMembers().Cast<ITypeSymbol>());

            foreach (var nestedNs in ns.GetNamespaceMembers())
            {
                CollectTypes(nestedNs, types);
            }
        }

        private static bool IsKeyWord(string identifier)
        {
            if (SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None
                || SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates an escaped identifier if the identifier is a keyword (or contextual keyword) in C#.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string CreateEscapedIdentifier(string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }
            return IsKeyWord(identifier) ? $"@{identifier}" : identifier;
        }

        public static bool IsValidNamespace(string namespaceName)
        {
            if (namespaceName == null)
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            if (IsKeyWord(namespaceName))
            {
                return false;
            }

            var parts = namespaceName.Split('.');
            foreach (var part in parts)
            {
                if (!SyntaxFacts.IsValidIdentifier(part))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidIdentifier(string identifier)
        {
            return SyntaxFacts.IsValidIdentifier(identifier);
        }
    }
}
