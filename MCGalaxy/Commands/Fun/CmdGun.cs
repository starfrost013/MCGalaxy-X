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
using MCGalaxy.Games;

namespace MCGalaxy.Commands.Fun {
    public sealed class CmdGun : Command2 {
        public override string name { get { return "GiveWeapon"; } }

        public override string shortcut { get { return "Gun";  } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public override bool SuperUseable { get { return false; } }

        public override void Use(Player p, string message, CommandData data)
        {
            if (!p.level.Config.Guns)
            {
                p.Message("Weapons cannot be used on this map!"); return;
            }

            if (p.weapon != null && message.Length == 0)
            {
                p.weapon.Disable(); return;
            }

            Weapon TheWeapon = GetGun(message);

            if (TheWeapon != null)
            {
                TheWeapon.Enable(p);
            }
            else
            {
                Help(p);
                return; 
            }
        }

        static Weapon GetGun(string MessageType)
        {
            WeaponType type = Weapon.ParseType(MessageType);

            switch (type)
            {
                case WeaponType.Destroy:
                    return new PenetrativeGun();
                case WeaponType.Teleport:
                    return new TeleportGun();
                case WeaponType.Explode:
                    return new ExplosiveGun();
                case WeaponType.Laser:
                    return new LaserGun();
                case WeaponType.Normal:
                    return new Gun();
                default:
                    return WeaponManager.GetWeaponWithName(MessageType);
            }

        }
        
        public override void Help(Player p)
        {
            p.Message("&T/GiveWeapon [at end] (alias: /gun)");
            p.Message("&HGives you a weapon.");
            p.Message("&HYou can type any weapon available on this server.");
            p.Message("&HAvailable [at end] types: &Sexplode, destroy, laser, tp, normal [TEMP]");
        }
    }
}
