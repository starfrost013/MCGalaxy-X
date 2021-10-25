using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// TCCSettlement
    /// 
    /// Defines a TownyCC settlement. A TownyCC settlement is a separate entity from a faction/whatever, and is essentially a 
    /// marked piece of land that is controlled by either nobody, or a faction/team.
    /// 
    /// They can be invaded, defended, and change hands. They can also be controlled by nobody, in which they can be claimed by a player, whose faction will then automatically 
    /// control the settlement.
    /// 
    /// They can be any size.
    /// </summary>
    public class TCCSettlement
    {

        /// <summary>
        /// The players that are a part of this faction.
        /// </summary>
        public VolatileArray<Player> Player { get; set; }

        /// <summary>
        /// The name of this faction. 
        /// </summary>
        public string Name { get; set; }

        #region may change this stuff
        /// <summary>
        /// Top left extents of this settlements' territory.
        /// </summary>
        public Position ExtentsAA { get; set; }

        /// <summary>
        /// Top right extents of this settlements' territory.
        /// </summary>
        public Position ExtentsAB { get; set; }

        /// <summary>
        /// Bottom left extents of this settlements' territory. 
        /// </summary>
        public Position ExtentsBA { get; set; }

        /// <summary>
        /// Bottom right extents of this settlements' territory.
        /// </summary>
        public Position ExtentsBB { get; set; }

        #endregion
    }
}
