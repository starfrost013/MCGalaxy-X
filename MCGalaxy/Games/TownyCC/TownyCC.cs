using MCGalaxy.Events.GameEvents; 
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
    public sealed partial class TownyCC : NoRoundsGame
    {
        public override string GameName => "TownyCC";
        public string GameVersion => "0.1.0";

        public static TownyCCConfig Config = new TownyCCConfig(); // constructor probably 

        /// <summary>
        /// TODO: This is dumb
        /// </summary>
        public static TownyCC Instance => new TownyCC();


        public override void Start(Player p, string Map)
        {
            Logger.Log(LogType.TCCSystem, $"TownyCC {GameVersion} starting...");
            base.Start(p, Map);
        }

        protected override void StartGame()
        {
            return; // empty for now
        }

        protected override void EndGame()
        {
            return; // empty for now
        }


        public override void End()
        {
            base.End();
            
        }

        public override BaseGameConfig GetConfig() { return Config; }

        public override void UpdateMapConfig()
        {
            return; // empty for now
        }

        protected override void DoRound()
        {
            return; // empty for now
        }

        protected override List<Player> GetPlayers()
        {
            return new List<Player>(); // empty for now
        }

        public override void OutputStatus(Player p)
        {
            return; // empty for now
        }

    }
}
