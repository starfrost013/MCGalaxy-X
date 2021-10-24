using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    public class TownyCCMapConfig : NoRoundsGameMapConfig
    {
        public override void Save(string map)
        {
            return; // empty for now
        }

        public override void Load(string map)
        {
            return; // empty for now
        }

        public override void SetDefaults(Level lvl)
        {
            return; // empty for now
        }
    }
}
