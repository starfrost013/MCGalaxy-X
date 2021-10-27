﻿/*
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
using MCGalaxy.Commands.World;

namespace MCGalaxy.Games {

    public abstract class Game
    {
        public Level Map;
        /// <summary> Whether this game is currently running/active </summary>
        public bool Running;
        /// <summary> Full name of this game (e.g. Zombie Survival) </summary>
        public abstract string GameName { get; }
        
        public static VolatileArray<Game> RunningGames = new VolatileArray<Game>(false);
        public static Game GameOn(Level lvl) {
            if (lvl == null) return null;
            Game[] games = RunningGames.Items;
            
            foreach (Game game in games) {
                if (game.Map == lvl) return game;
            }
            return null;
        }

        /// <summary> Whether this game intercepts the given chat message </summary>
        /// <example> RoundsGame uses this when voting for next level at end of rounds </example>
        public virtual bool HandlesChatMessage(Player p, string message) { return false; }
        public virtual void PlayerJoinedGame(Player p) { }
        public virtual void PlayerLeftGame(Player p) { }
        
        /// <summary> Adjusts the given player's prefix that appears in all chat messages </summary>
        /// <example> Zombie Survival uses this to show winstreaks </example>
        public virtual void AdjustPrefix(Player p, ref string prefix) { }
        /// <summary> Immediately force stops/ends this game </summary>
        public abstract void End();
        
        
        /// <summary> Resets all CPE status messages to blank. </summary>
        protected void ResetStatus(Player p) {
            p.SendCpeMessage(CpeMessageType.Status1, "");
            p.SendCpeMessage(CpeMessageType.Status2, "");
            p.SendCpeMessage(CpeMessageType.Status3, "");
        }
        
        /// <summary> Sends a message of the given type to all players on the level this game is running on. </summary>
        public void MessageMap(CpeMessageType type, string message)
        {
            if (!Running) return;
            Player[] online = PlayerInfo.Online.Items;
            
            foreach (Player p in online) {
                if (p.level != Map) continue;
                p.SendCpeMessage(type, message);
            }
        }
        
        /// <summary> Sends CPE Status1 messages to all players in this game's current level </summary>
        public void UpdateAllStatus1() { UpdateAllStatus(CpeMessageType.Status1); }
        /// <summary> Sends CPE Status2 messages to all players in this game's current level </summary>
        public void UpdateAllStatus2() { UpdateAllStatus(CpeMessageType.Status2); }
        /// <summary> Sends CPE Status3 messages to all players in this game's current level </summary>
        public void UpdateAllStatus3() { UpdateAllStatus(CpeMessageType.Status3); }
        
        /// <summary> Sends CPE Status1, Status2, and Status3 messages to all players in this game's current level </summary>
        public void UpdateAllStatus() {
            UpdateAllStatus1();
            UpdateAllStatus2();
            UpdateAllStatus3();
        }
        
        void UpdateAllStatus(CpeMessageType status) {
            Player[] online = PlayerInfo.Online.Items;
            foreach (Player p in online) {
                if (p.level != Map) continue;
                
                string msg = status == CpeMessageType.Status1 ? FormatStatus1(p) :
                    (status == CpeMessageType.Status2 ? FormatStatus2(p) : FormatStatus3(p));
                p.SendCpeMessage(status, msg);
            }
        }
        
        
        protected virtual string FormatStatus1(Player p) { return ""; }
        protected virtual string FormatStatus2(Player p) { return ""; }
        protected virtual string FormatStatus3(Player p) { return ""; }
        
        /// <summary> Sends a CPE Status1 message (using FormatStatus1) to the given player </summary>
        protected void UpdateStatus1(Player p) {
            p.SendCpeMessage(CpeMessageType.Status1, FormatStatus1(p));
        }
        
        /// <summary> Sends a CPE Status2 message (using FormatStatus2) to the given player </summary>
        protected void UpdateStatus2(Player p) {
            p.SendCpeMessage(CpeMessageType.Status2, FormatStatus2(p));
        }
        
        /// <summary> Sends a CPE Status3 message (using FormatStatus3) to the given player </summary>
        protected void UpdateStatus3(Player p) {
            p.SendCpeMessage(CpeMessageType.Status3, FormatStatus3(p));
        }
        
        /// <summary> Resets all CPE Status messages to blank for the given player </summary>
        protected void ResetStatus(Player p) {
            p.SendCpeMessage(CpeMessageType.Status1, "");
            p.SendCpeMessage(CpeMessageType.Status2, "");
            p.SendCpeMessage(CpeMessageType.Status3, "");
        }
        
        
        public static bool InRange(Player a, Player b, int dist) {
            int dx = Math.Abs(a.Pos.X - b.Pos.X);
            int dy = Math.Abs(a.Pos.Y - b.Pos.Y);
            int dz = Math.Abs(a.Pos.Z - b.Pos.Z);
            return dx <= dist && dy <= dist && dz <= dist;
        }
    }
}
