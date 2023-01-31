// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore
{
    public interface IModelMetadata
    {
        IPropertyMetadata[] Properties { get; }
        IPropertyMetadata[] PrimaryKeys { get; }
        INavigationMetadata[] Navigations { get; }
        Type ModelType { get; }
        string EntitySetName { get; }
    }
}
