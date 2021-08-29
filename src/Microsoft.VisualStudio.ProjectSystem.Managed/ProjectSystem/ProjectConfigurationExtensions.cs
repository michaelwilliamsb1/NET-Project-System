﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.ProjectSystem
{
    internal static class ProjectConfigurationExtensions
    {
        /// <summary>
        /// Returns true if this is a cross-targeting project configuration with a "TargetFramework" dimension.
        /// This is true for a project with an explicit "TargetFrameworks" property, but no "TargetFrameworkMoniker" or "TargetFramework" property.
        /// </summary>
        /// <param name="projectConfiguration"></param>
        internal static bool IsCrossTargeting(this ProjectConfiguration projectConfiguration)
        {
            return projectConfiguration.Dimensions.ContainsKey(ConfigurationGeneral.TargetFrameworkProperty);
        }

        /// <summary>
        /// Returns true if the given project configurations are equal ignoring the "TargetFramework" dimension.
        /// </summary>
        internal static bool EqualIgnoringTargetFramework(this ProjectConfiguration projectConfiguration1, ProjectConfiguration projectConfiguration2)
        {
            Requires.NotNull(projectConfiguration1, nameof(projectConfiguration1));
            Requires.NotNull(projectConfiguration2, nameof(projectConfiguration2));

            if (projectConfiguration1.Dimensions.Count != projectConfiguration2.Dimensions.Count)
            {
                return false;
            }

            if (!projectConfiguration1.IsCrossTargeting() || !projectConfiguration2.IsCrossTargeting())
            {
                return projectConfiguration1.Equals(projectConfiguration2);
            }

            foreach ((string dimensionName, string dimensionValue) in projectConfiguration1.Dimensions)
            {
                // Ignore the TargetFramework.
                if (StringComparers.ConfigurationDimensionNames.Equals(dimensionName, ConfigurationGeneral.TargetFrameworkProperty))
                {
                    continue;
                }

                // Dimension values must be compared in a case-sensitive manner.
                if (!projectConfiguration2.Dimensions.TryGetValue(dimensionName, out string activeValue) ||
                    !string.Equals(dimensionValue, activeValue, StringComparisons.ConfigurationDimensionNames))
                {
                    return false;
                }
            }

            return true;
        }
    }
}