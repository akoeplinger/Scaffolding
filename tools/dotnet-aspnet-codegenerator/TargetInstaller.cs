// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Reflection;

namespace Microsoft.VisualStudio.Web.CodeGeneration.Tools
{
    public static class TargetInstaller
    {
        /// <summary>
        /// Check given the project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="objFolder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool EnsureTargetImported(string projectName, string objFolder)
        {
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(objFolder))
            {
                return false;
            }

            const string ToolsImportTargetsName = "Imports.targets";
            // Create the directory structure for obj folder if it doesn't exist.
            Directory.CreateDirectory(objFolder);

            var fileName = $"{projectName}.codegeneration.targets";
            var importingTargetFilePath = Path.Combine(objFolder, fileName);

            if (File.Exists(importingTargetFilePath))
            {
                return true;
            }

            var toolType = typeof(TargetInstaller);
            var toolAssembly = toolType.GetTypeInfo().Assembly;
            var toolNamespace = toolType.Namespace;
            var toolImportTargetsResourceName = $"{toolNamespace}.compiler.resources.{ToolsImportTargetsName}";

            using (var stream = toolAssembly.GetManifestResourceStream(toolImportTargetsResourceName))
            {
                var targetBytes = new byte[stream.Length];
                stream.Read(targetBytes, 0, targetBytes.Length);
                File.WriteAllBytes(importingTargetFilePath, targetBytes);
            }
            return true;
        }
    }
}
