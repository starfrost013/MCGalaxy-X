using MCGalaxy.Events.GameEvents; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// BaseGame
    /// 
    /// October 24, 2021
    /// 
    /// Base class for all MCGalaxy games.
    /// Implements the very minimum required for a functional game. 
    /// </summary>
    public abstract partial class BaseGame : Game
    {
        /// <summary> The instance of this game's overall configuration object. </summary>
        public abstract BaseGameConfig GetConfig();

        /// <summary> Saves stats to the database for the given player. </summary>
        protected virtual void SaveStats(Player pl) { }

        /// <summary> Updates state from the map specific configuration file. </summary>
        public abstract void UpdateMapConfig();

        /// <summary> Messages general info about current round and players. </summary>
        /// <remarks> e.g. who is alive, points of each team, etc. </remarks>
        public abstract void OutputStatus(Player p);

        // these should probably be properties
        public LevelPicker Picker;
        public string LastMap = "";

        /// <summary>
        /// The name of thi game.
        /// </summary>
        public override string GameName => "BaseGame";

        public BaseGame()
        {
            Picker = new LevelPicker();
        }

        public override void End()
        {
            if (!Running) return;
            Running = false;
            RunningGames.Remove(this);
            OnStateChangedEvent.Call(this);
            
        }

       public virtual void Start(Player p, string map)
       {
            string StartMapName = GetStartMap(p, map);

            if (StartMapName == null || StartMapName.Length == 0) p.Message($"No maps have been set up for {GameName} yet!");
            if (!SetMap(StartMapName)) p.Message("Failed to load initial map!");

            Chat.MessageGlobal("{0} is starting on {1}&S!", GameName, Map.ColoredName);
            Logger.Log(LogType.GameActivity, "{0} game started", GameName);

            Running = true;
            RunningGames.Add(this); // todo; add before start() is called
            OnStateChangedEvent.Call(this);

            // Register event handlers.
            HookEventHandlers();

            Thread GameThread = new Thread(RunGame);
            GameThread.Name = $"Game_{GameName}";
            GameThread.Start(); 
       }

        protected virtual string GetStartMap(Player p, string forcedMap)
        {
            if (forcedMap.Length > 0) return forcedMap;
            List<string> maps = Picker.GetCandidateMaps(this);

            if (maps == null || maps.Count == 0) return null;
            return LevelPicker.GetRandomMap(new Random(), maps);
        }


        public virtual void RunGame()
       {
            while (Running)
            {

            }

            End(); 
       }

       protected virtual bool SetMap(string map)
       {
           Picker.QueuedMap = null;
           Level next = LevelInfo.FindExact(map);

           if (next == null) next = LevelActions.Load(Player.Console, map, false);
           if (next == null) return false;

           Map = next;
           Map.SaveChanges = false;

           if (GetConfig().SetMainLevel) Server.SetMainLevel(Map);
           UpdateMapConfig();
           return true;
        }

        public override bool HandlesChatMessage(Player p, string message)
        {
            return base.HandlesChatMessage(p, message);
        }

        public override void PlayerJoinedGame(Player p)
        {
            base.PlayerJoinedGame(p);
        }

        public override void PlayerLeftGame(Player p)
        {
            base.PlayerLeftGame(p);
        }

        protected abstract void StartGame();


    }
}
