// <copyright file="GlobalSuppressions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

// Note, if https://github.com/SpecFlowOSS/SpecFlow/pull/1790 is ever successfully merged,
// we should be able to take this back out again.
[assembly: SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1638:File header file name documentation should match file name",
    Justification = "This file is generated as part of SpecFlow's build process, so the warning is spurious. (SpecFlow should really add the necessary file header to prevent this warning.)",
    Scope = "type",
    Target = "SpecFlow_GeneratedTests_NUnitAssemblyHooks")]