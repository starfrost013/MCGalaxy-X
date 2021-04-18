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
using System.Threading;
using MCGalaxy.Config;
using MCGalaxy.Events.GroupEvents;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Events.ServerEvents;

namespace MCGalaxy.Modules.Relay.Discord {

    public sealed class DiscordBot : RelayBot {
        bool disconnected;
        string[] operatorIds;
        
        DiscordApiClient api;
        DiscordWebsocket socket;
        DiscordConfig config;
        Thread thread;
        
        public override string RelayName { get { return "Discord"; } }        
        public override bool Enabled { get { return config.Enabled; } }
        public override bool Connected { get { return !disconnected; } }
        
        
        public void RunAsync(DiscordConfig conf) {
            config = conf;
            socket = new DiscordWebsocket();
            
            Channels    = conf.Channels.SplitComma();
            OpChannels  = conf.OpChannels.SplitComma();
            operatorIds = conf.OperatorUsers.SplitComma();
            
            socket.Token     = config.BotToken;
            socket.Handler   = HandleEvent;
            socket.GetStatus = GetStatus;
            socket.OnReady   = OnReady;
                
            thread      = new Thread(IOThread);
            thread.Name = "DiscordRelayBot";
            thread.IsBackground = true;
            thread.Start();
        }
        
        void IOThread() {
            try {
                socket.Connect();
                socket.ReadLoop();
            } catch (Exception ex) {
                Logger.LogError("Discord relay error", ex);
            }
        }
        
        protected override void DoConnect() {
            // TODO implement
            disconnecting = false;
            throw new NotImplementedException();
        }
        
        volatile bool disconnecting;
        protected override void DoDisconnect(string reason) {
            if (disconnecting) return;
            disconnecting = true;
            
            try {
                socket.Disconnect();
            } finally {
                disconnected = true;
                UnregisterEvents();
            }
        }
        
        
        void HandleEvent(JsonObject obj) {
            // actually handle the event
            string eventName = (string)obj["t"];
            if (eventName == "MESSAGE_CREATE") HandleMessageEvent(obj);
        }
        
        void HandleMessageEvent(JsonObject obj) {
            JsonObject data   = (JsonObject)obj["d"];
            JsonObject author = (JsonObject)data["author"];
            string channel    = (string)data["channel_id"];
            string message    = (string)data["content"];
            
            RelayUser user = new RelayUser();
            user.Nick   = (string)author["username"];
            user.UserID = (string)author["id"];            
            HandleChannelMessage(user, channel, message);
        }
        
        string GetStatus() {
            string online = PlayerInfo.NonHiddenCount().ToString();
            return config.Status.Replace("{PLAYERS}", online);
        }        
        
        void OnReady() {
            api = new DiscordApiClient();
            api.Token = config.BotToken;
            RegisterEvents();
        }
        
        
        void RegisterEvents() {
            OnPlayerConnectEvent.Register(HandlePlayerConnect, Priority.Low);
            OnPlayerDisconnectEvent.Register(HandlePlayerDisconnect, Priority.Low);
            OnPlayerActionEvent.Register(HandlePlayerAction, Priority.Low);
            HookEvents();
        }
        
        void UnregisterEvents() {
            OnPlayerConnectEvent.Unregister(HandlePlayerConnect);
            OnPlayerDisconnectEvent.Unregister(HandlePlayerDisconnect);
            OnPlayerActionEvent.Unregister(HandlePlayerAction);
            UnhookEvents();
        }
        
        void HandlePlayerConnect(Player p) { socket.SendUpdateStatus(); }
        void HandlePlayerDisconnect(Player p, string reason) { socket.SendUpdateStatus(); }
        
        
        public override void MessageChannel(string channel, string message) {
            message = EmotesHandler.Replace(message);
            message = ChatTokens.ApplyCustom(message);
            message = Colors.StripUsed(message);
            api.SendMessage(channel, message);
        }
        
        public override void MessageUser(RelayUser user, string message) {
            // TODO: implement this
        }
        
        void HandlePlayerAction(Player p, PlayerAction action, string message, bool stealth) {
            if (action != PlayerAction.Hide && action != PlayerAction.Unhide) return;
            socket.SendUpdateStatus();
        } 
        
        protected override bool CanUseCommands(RelayUser user, string cmdName, out string error) {
            error = null;
            return user.UserID != null && 
                operatorIds.CaselessContains(user.UserID);
        }
    }
}
