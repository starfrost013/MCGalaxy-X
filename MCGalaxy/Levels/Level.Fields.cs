﻿/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
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
using System.Threading;
using MCGalaxy.Blocks;
using MCGalaxy.Blocks.Physics;
using MCGalaxy.DB;
using MCGalaxy.Config;
using MCGalaxy.Games;
using MCGalaxy.Maths;
using MCGalaxy.Network;
using MCGalaxy.Util;

namespace MCGalaxy {
    public sealed partial class Level : IDisposable {

        /// <summary>
        /// The name of the map file, sans extension.
        /// </summary>
        public string MapName;
        /// <summary>
        /// Same as MapName, unless <cref>IsMuseum</cref>, then it will be prefixed and suffixed to denote museum.
        /// </summary>
        public string name;

        public string Color { get { return Config.Color; } }
        public string ColoredName { get { return Config.Color + name; } }
        public LevelConfig Config = new LevelConfig();
        
        public byte rotx, roty;
        public ushort spawnx, spawny, spawnz;
        public Position SpawnPos { get { return new Position(16 + spawnx * 32, 32 + spawny * 32, 16 + spawnz * 32); } }
            
        public BlockDefinition[] CustomBlockDefs = new BlockDefinition[Block.ExtendedCount];
        public BlockProps[] Props = new BlockProps[Block.ExtendedCount];
        public ExtrasCollection Extras = new ExtrasCollection();
        public VolatileArray<PlayerBot> Bots = new VolatileArray<PlayerBot>(false);
        bool unloadedBots;
        
        public HandleDelete[] DeleteHandlers = new HandleDelete[Block.ExtendedCount];
        public HandlePlace[] PlaceHandlers = new HandlePlace[Block.ExtendedCount];
        public HandleWalkthrough[] WalkthroughHandlers = new HandleWalkthrough[Block.ExtendedCount];
        public HandlePhysics[] PhysicsHandlers = new HandlePhysics[Block.ExtendedCount];
        internal HandlePhysics[] physicsDoorsHandlers = new HandlePhysics[Block.ExtendedCount];
        internal AABB[] blockAABBs = new AABB[Block.ExtendedCount];
        
        public ushort Width, Height, Length;
        /// <summary> Whether this level should be treated as a readonly museum </summary>
        public bool IsMuseum;

        public int ReloadThreshold {
            get { return Math.Max(10000, (int)(Server.Config.DrawReloadThreshold * Width * Height * Length)); }
        }
        
        /// <summary> Maximum valid X coordinate (Width - 1) </summary>
        public int MaxX { get { return Width  - 1; } }
        /// <summary> Maximum valid Y coordinate (Height - 1) </summary>
        public int MaxY { get { return Height - 1; } }
        /// <summary> Maximum valid Z coordinate (Length - 1) </summary>
        public int MaxZ { get { return Length - 1; } }
        
        public bool Changed;
         /// <summary> Whether block changes made on this level should be saved to the BlockDB and .lvl files. </summary>
        public bool SaveChanges = true;
        public bool ChangedSinceBackup;
        
        /// <summary> Whether players on this level sees server-wide chat. </summary>
        public bool SeesServerWideChat { get { return Config.ServerWideChat && Server.Config.ServerWideChat; } }
        
        internal readonly object saveLock = new object(), botsIOLock = new object();
        public BlockQueue blockqueue = new BlockQueue();
        BufferedBlockSender bulkSender;

        public List<UndoPos> UndoBuffer = new List<UndoPos>();
        public VolatileArray<Zone> Zones = new VolatileArray<Zone>();
        public BlockDB BlockDB;
        public LevelAccessController VisitAccess, BuildAccess;
        
        // Physics fields and settings
        public int physics { get { return Physicsint; } }
        int Physicsint;
        public int currentUndo;
        
        public int lastCheck, lastUpdate;
        internal FastList<Check> ListCheck = new FastList<Check>(); //A list of blocks that need to be updated
        internal FastList<Update> ListUpdate = new FastList<Update>(); //A list of block to change after calculation
        internal SparseBitSet listCheckExists, listUpdateExists;
        
        public Random physRandom = new Random();
        public bool PhysicsPaused;
        Thread physThread;
        readonly object physThreadLock = new object();
        internal readonly object physTickLock = new object();
        bool physThreadStarted = false;
        
        public List<C4Data> C4list = new List<C4Data>();

        public bool CanPlace  { get { return Config.Buildable && Config.BuildType != BuildType.NoModify; } }
        public bool CanDelete { get { return Config.Deletable && Config.BuildType != BuildType.NoModify; } }

        public int WinChance {
            get { return Config.RoundsPlayed == 0 ? 100 : (Config.RoundsHumanWon * 100) / Config.RoundsPlayed; }
        }
        
        internal bool hasPortals, hasMessageBlocks;
    }
}