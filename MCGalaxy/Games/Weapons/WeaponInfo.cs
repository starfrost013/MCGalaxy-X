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
        private static string PropertiesDirectory = @"properties\weapons";
        public static string GetPropertiesPath(Weapon Weapon) => $@"{PropertiesDirectory}\{Weapon.Name}.properties";
    }
}
