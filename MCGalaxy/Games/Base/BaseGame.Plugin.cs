using MCGalaxy.Events.LevelEvents;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Events.PlayerDBEvents;
using MCGalaxy.Events.ServerEvents;
using MCGalaxy.Network; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    public abstract partial class BaseGame : Game
    {

        protected virtual void HookEventHandlers()
        {
            OnLevelUnloadEvent.Register(HandleLevelUnload, Priority.High);
            OnSendingHeartbeatEvent.Register(HandleSendingHeartbeat, Priority.High);
            OnInfoSaveEvent.Register(HandleSaveStats, Priority.High);

            OnPlayerActionEvent.Register(HandlePlayerAction, Priority.High);
            OnPlayerDisconnectEvent.Register(HandlePlayerDisconnect, Priority.High);
        }

        protected virtual void UnhookEventHandlers()
        {
            OnLevelUnloadEvent.Unregister(HandleLevelUnload);
            OnSendingHeartbeatEvent.Unregister(HandleSendingHeartbeat);
            OnInfoSaveEvent.Unregister(HandleSaveStats);

            OnPlayerActionEvent.Unregister(HandlePlayerAction);
            OnPlayerDisconnectEvent.Unregister(HandlePlayerDisconnect);
        }
        void HandleSaveStats(Player p, ref bool cancel) { SaveStats(p); }


        protected virtual void HandleSendingHeartbeat(Heartbeat service, ref string name)
        {
            if (Map == null || !GetConfig().MapInHeartbeat) return;
            name += " (map: " + Map.MapName + ")";
        }

        protected virtual void HandlePlayerDisconnect(Player p, string reason)
        {
            if (p.level != Map) return;
            PlayerLeftGame(p);
        }

        protected void HandleJoinedCommon(Player p, Level prevLevel, Level level, ref bool announce)
        {
            if (prevLevel == Map && level != Map)
            {
                if (Picker.Voting) Picker.ResetVoteMessage(p);
                ResetStatus(p);
                PlayerLeftGame(p);
            }
            else if (level == Map)
            {
                if (Picker.Voting) Picker.SendVoteMessage(p);
                UpdateStatus1(p); UpdateStatus2(p); UpdateStatus3(p);
            }

            if (level != Map) return;

            if (prevLevel == Map || LastMap.Length == 0)
            {
                announce = false;
            }
            else if (prevLevel != null && prevLevel.name.CaselessEq(LastMap))
            {
                // prevLevel is null when player joins main map
                announce = false;
            }
        }

        protected void MessageMapInfo(Player p)
        {
            p.Message("This map has &a{0} likes &Sand &c{1} dislikes",
                           Map.Config.Likes, Map.Config.Dislikes);
            string[] authors = Map.Config.Authors.SplitComma();
            if (authors.Length == 0) return;

            p.Message("It was created by {0}", authors.Join(n => p.FormatNick(n)));
        }

        protected void HandleLevelUnload(Level lvl, ref bool cancel)
        {
            if (lvl != Map) return;
            Logger.Log(LogType.GameActivity, "Unload cancelled! A {0} game is currently going on!", GameName);
            cancel = true;
        }

        protected void HandlePlayerAction(Player p, PlayerAction action, string message, bool stealth)
        {
            if (!(action == PlayerAction.Referee || action == PlayerAction.UnReferee)) return;
            if (p.level != Map) return;

            if (action == PlayerAction.UnReferee)
            {
                PlayerActions.Respawn(p);
                PlayerJoinedGame(p);
                p.GameProperties.Referee = false;
            }
            else
            {
                PlayerLeftGame(p);
                p.GameProperties.Referee = true;
                Entities.GlobalDespawn(p, false, false);
            }

            Entities.GlobalSpawn(p, false, "");
            TabList.Update(p, true);
        }
    }



}
