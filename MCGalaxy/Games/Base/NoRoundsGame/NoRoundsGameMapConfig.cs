using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary> Stores map-specific game configuration state. </summary>
    public abstract class NoRoundsGameMapConfig
    {

        protected void LoadFrom(ConfigElement[] cfg, string propsDir, string map)
        {
            string path = propsDir + map + ".properties";
            ConfigElement.ParseFile(cfg, path, this);
        }

        protected void SaveTo(ConfigElement[] cfg, string propsDir, string map)
        {
            string path = propsDir + map + ".properties";
            if (!Directory.Exists(propsDir)) Directory.CreateDirectory(propsDir);
            ConfigElement.SerialiseSimple(cfg, path, this);
        }

        /// <summary> Saves this configuration to disc. </summary>
        public abstract void Save(string map);
        /// <summary> Loads this configuration from disc. </summary>
        public abstract void Load(string map);
        /// <summary> Applies default values for config fields which differ per map. </summary>
        /// <remarks> e.g. spawn positions, zones </remarks>
        public abstract void SetDefaults(Level lvl);
    }
}
