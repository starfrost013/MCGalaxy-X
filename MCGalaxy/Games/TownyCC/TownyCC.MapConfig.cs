using MCGalaxy.Config; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    public class TownyCCMapConfig : NoRoundsGameMapConfig
    {
        // please note that config must be fields due to how it's loaded (fix this?)

        /// <summary>
        /// Maximum towns/settlements allowed in this TownyCC map. 
        /// </summary>
        [ConfigInt("MaxSettlements", "Game", 10, 2)] 
        public int MaxSettlements;

        static ConfigElement[] Config;

        private string PropertiesDir => "properties/TownyCC/";

        public override void Save(string map)
        {
            if (Config == null) Config = ConfigElement.GetAll(typeof(TownyCCMapConfig));
            SaveTo(Config, PropertiesDir, map);

            return; // empty for now
        }

        public override void Load(string map)
        {
            if (Config == null) Config = ConfigElement.GetAll(typeof(TownyCCMapConfig));
            LoadFrom(Config, PropertiesDir, map);
            return; // empty for now
        }

        public override void SetDefaults(Level lvl)
        {
            return; // empty for now
        }
    }
}
