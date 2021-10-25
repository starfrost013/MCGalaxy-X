using MCGalaxy.Config; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// WeaponConfig
    /// 
    /// Defines the weapon configuration.
    /// </summary>
    public class WeaponConfig
    {
        public ConfigElement[] Config;

        // pretty sure due to the way its loaded all config stuff has to be fields

        [ConfigString("Name", "Main", "Weapon Name Undefined - Please Set")]
        public string Name;

        [ConfigInt("Name", "Main", 20)]
        public int MaximumDamage;

        // A formula will be used in order to usually award "close" to the maximum damage

        [ConfigDouble("MinimumDamagePercentage", "Main", 0.7 )]
        /// <summary>
        /// Minimum damage percentage.
        /// </summary>
        public double MinimumDamagePercentage; 
    }
}
