using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// TownyCC test
    /// 
    /// October 2021, starfrost
    /// </summary>
    public partial class TownyCCConfig : NoRoundsGameConfig
    {

        public override bool AllowAutoload => false;

        protected override string GameName { get { return "TownyCC [DEBUG]"; } }

        /// <summary>
        /// Path to the properties for this Config.
        /// 
        /// temp: until tccxml
        /// </summary>
        protected override string PropsPath { get { return "properties/TownyCC/main.properties "; } }

    }
}
