using MCGalaxy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// Faction
    /// 
    /// Defines a faction/team/I don't know the name yet in TownyCC.
    /// 
    /// A faction is a group of players in a team, similar to in actual Minecraft. They can declare wars with other factions,
    /// where - to start with, although multiple modes will be implemented (configurable per-map) - the primary task will be to kill all of the other players 
    /// using weapons, which will extend the MCGalaxy weapon system. (Hand?)
    /// 
    /// To achieve this the player object will be expanded with survival-like stuff, and the weapon system will be overhauled to allow you to find and get weapons.
    /// A /giveweapon command and other administration commads will also be implemented.
    /// 
    /// All factions are saved in the DB (or to xml?)
    /// </summary>
    public class TCCFaction
    {
        /// <summary>
        /// The members of this faction.
        /// </summary>
        public VolatileArray<Player> Members { get; set; }
        
        /// <summary>
        /// The members of this town.
        /// </summary>
        public string Name { get; set; }

        #region may change this stuff
        /// <summary>
        /// Top left extents of this factions' territory.
        /// </summary>
        public Position ExtentsAA { get; set; }

        /// <summary>
        /// Top right extents of this factions' territory.
        /// </summary>
        public Position ExtentsAB { get; set; }

        /// <summary>
        /// Bottom left extents of this factions' territory. 
        /// </summary>
        public Position ExtentsBA { get; set; }

        /// <summary>
        /// Bottom right extents of this factions' territory.
        /// </summary>
        public Position ExtentsBB { get; set; }

        #endregion
    }
}
