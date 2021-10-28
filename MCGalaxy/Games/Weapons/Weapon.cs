/*
    Copyright 2011 MCForge
        
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
using MCGalaxy.Commands;
using MCGalaxy.Config;
using MCGalaxy.Eco;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using MCGalaxy.Tasks;
using BlockID = System.UInt16;

namespace MCGalaxy.Games {

    /// <summary> Represents a weapon which can interact with blocks or players until it dies. </summary>
    public class Weapon //: Item
    {

        [Obsolete("Use Config.Name instead")]
        public virtual string Name { get; }
        static bool hookedEvents;
        
        protected Player p;
        AimBox aimer;

        public WeaponConfig Config; // perhaps make a property?

        /// <summary>
        /// The filename of this weapon.
        /// </summary>
        public string FileName { get; set; }

        public Weapon()
        {
            Config = new WeaponConfig();
        }

        public WeaponConfig GetConfig() => Config; 
        /// <summary> Applies this weapon to the given player, and sets up necessary state. </summary>
        public virtual void Enable(Player p)
        {
            if (!hookedEvents) {
                OnPlayerClickEvent.Register(PlayerClickCallback, Priority.Low);
                OnBlockChangingEvent.Register(BlockChangingCallback, Priority.Low);
                hookedEvents = true;
            }
            
            this.p   = p;
            p.ClearBlockchange();
            p.weapon = this;
            
            if (p.Supports(CpeExt.PlayerClick))
            {
                if (Config.Name == null)
                {
                    p.Message(Name + " engaged, click to fire at will");
                }
                else
                {
                    p.Message(Config.Name + " engaged, click to fire at will");
                }
            }
            else
            {             
                if (Config.Name == null)
                {
                    p.Message(Name + " engaged, fire at will");
                }
                else
                {
                    p.Message(Config.Name + " engaged, fire at will");
                }
                p.aiming = true;
                aimer = new AimBox();
                aimer.Hook(p);
            }
        }

        public virtual void Disable()
        {
            p.aiming = false;

            if (Config.Name == null)
            {
                p.Message($"{Name} disabled");
            }
            else
            {
                p.Message($"{Config.Name} disabled");
            }
            
            p.weapon = null;
        }
        
        /// <summary> Called when the player fires this weapon. </summary>
        /// <remarks> Activated by clicking through either PlayerClick or on a glass box around the player. </remarks>
        protected virtual void OnActivated(Vec3F32 dir, BlockID block)
        {
            return; 
        }

        
        static void BlockChangingCallback(Player p, ushort x, ushort y, ushort z, BlockID block, bool placing, ref bool cancel) {
            Weapon weapon = p.weapon;
            if (weapon == null) return;
            
            // Revert block back since client assumes changes always succeeds
            p.RevertBlock(x, y, z);
            cancel = true;
            
            // Defer to player click handler if PlayerClick supported
            if (weapon.aimer == null) return;
            
            if (!p.level.Config.Guns) { weapon.Disable(); return; }
            if (!CommandParser.IsBlockAllowed(p, "use", block)) return;

            Vec3F32 dir = DirUtils.GetDirVector(p.Rot.RotY, p.Rot.HeadX);
            weapon.OnActivated(dir, block);
        }
        
        static void PlayerClickCallback(Player p, MouseButton btn, MouseAction action,
                                        ushort yaw, ushort pitch, byte entity,
                                        ushort x, ushort y, ushort z, TargetBlockFace face) {
            Weapon weapon = p.weapon;
            if (weapon == null || action != MouseAction.Pressed) return;
            
            if (!(btn == MouseButton.Left || btn == MouseButton.Right)) return;
            if (!p.level.Config.Guns) { weapon.Disable(); return; }
            
            BlockID held = p.ClientHeldBlock;
            if (!CommandParser.IsBlockAllowed(p, "use", held)) return;
            
            Vec3F32 dir = DirUtils.GetDirVectorExt(yaw, pitch);
            weapon.OnActivated(dir, held);
        }
        
        protected static Player PlayerAt(Player p, Vec3U16 pos, bool skipSelf)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player pl in players) {
                if (pl.level != p.level) continue;
                if (p == pl && skipSelf) continue;
                
                if (Math.Abs(pl.Pos.BlockX - pos.X)    <= 1
                    && Math.Abs(pl.Pos.BlockY - pos.Y) <= 1
                    && Math.Abs(pl.Pos.BlockZ - pos.Z) <= 1)
                {
                    return pl;
                }
            }
            return null;
        }
        
        public static WeaponType ParseType(string type) {
            if (type.Length == 0) return WeaponType.Normal;
            if (type.CaselessEq("destroy")) return WeaponType.Destroy;
            if (type.CaselessEq("tp") || type.CaselessEq("teleport")) return WeaponType.Teleport;
            if (type.CaselessEq("explode")) return WeaponType.Explode;
            if (type.CaselessEq("laser"))   return WeaponType.Laser;
            if (type.CaselessEq("melee")) return WeaponType.Melee;
            if (type.CaselessEq("ranged")) return WeaponType.Ranged;
            if (type.CaselessEq("normal")) return WeaponType.Normal; // temporary hack for the new weapons system
            return WeaponType.Invalid;
        }

        /// <summary>
        /// Loads this weapon.
        /// </summary>
        public void Load(string FileName = null)
        {
            if (FileName == null)
            {
                Config.Load(Config.Name);
            }
            else
            {
                Config.Load(FileName);
            }
            
        }

        public void Save(string FileName = null)
        {
            if (FileName == null)
            {
                Config.Save(Config.Name);
            }
            else
            {
                Config.Save(FileName);
            }
        }
    }
    
   
    
}
