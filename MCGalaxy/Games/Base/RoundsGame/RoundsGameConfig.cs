/*
    Copyright 2015 MCGalaxy
        
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.Collections.Generic;
using System.IO;
using MCGalaxy.Config;
using MCGalaxy.Events.GameEvents;
using MCGalaxy.Events.LevelEvents;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Events.ServerEvents;
using MCGalaxy.Network;

namespace MCGalaxy.Games {

    /// <summary> Stores map-specific game configuration state. </summary>
    public abstract class RoundsGameMapConfig : BaseGameMapConfig
    {
        // empty for now, may be expanded in future so keep
    }

    /// <summary> Stores overall game configuration state. </summary>
    public abstract class RoundsGameConfig : BaseGameConfig
    {
        // empty for now, may be expanded in future so keep
    }
}
