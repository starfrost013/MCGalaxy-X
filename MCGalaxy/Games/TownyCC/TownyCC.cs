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
    public sealed partial class TownyCC : Game
    {
        public override string GameName => "TownyCC";
        public string GameVersion => "0.1.0";

        public void Start()
        {
            Logger.Log(LogType.TCCSystem, $"TownyCC {GameVersion} starting...");
            Running = true;
            RunningGames.Add(this);
            OnStateChangedEvent.Call(this);
        }

        public override void End()
        {
            if (!Running) return;
            RunningGames.Remove(this);
            OnStateChangedEvent.Call(this);
            
        }

       


    }
}
