using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// Static class for obtaining weapon information.
    /// </summary>
    public static class WeaponInfo
    {
        internal static string PropertiesDirectory = @"properties\weapons";
        public static string GetPropertiesPath(string Name) => $@"{PropertiesDirectory}\{Name}.properties"; // probably going to move this
    }
}
