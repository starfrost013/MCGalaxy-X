using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection; 
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// WeaponManager
    /// 
    /// Static weapon manager - TEMPORARY? as we may put this in DB
    /// </summary>
    public static class WeaponManager
    {
        public static VolatileArray<Weapon> Weapons = new VolatileArray<Weapon>(); 
        
        public static void Load()
        {
            SaveDefault(); 
            DoLoad(); 
            
        }

        private static void SaveDefault()
        {
            AddWeapon(new Weapon { Config = new WeaponConfig { Name = "TestWeapon" } });

        }

        private static void DoLoad()
        {
            Logger.Log(LogType.SystemActivity, "Loading weapons...");

            // Recursively load every weapon from a file

            // todo: the actual recursion
            string[] FileNames = Directory.GetFiles(WeaponInfo.PropertiesDirectory); // maybe move this to here

            foreach (string FileName in FileNames)
            {
                Weapon Weapon = new Weapon();

                try
                {
                    if (!Weapons.Add(Weapon))
                    {
                        Logger.Log(LogType.Error, "Weapon loading aborted - error adding weapon");
                        return;
                    }

                    else
                    {
                        Weapon.FileName = FileName;
                        Weapon.Load(FileName);
                        Logger.Log(LogType.SystemActivity, $"Weapon {Weapon.Config.Name} loaded");
                    }

                }
                catch
                {
                    Logger.Log(LogType.Error, "Weapon loading aborted - error adding weapon");
                    return;
                }

            }

            Logger.Log(LogType.SystemActivity, $"Loaded {Weapons.Count} weapons");
        }

        /// <summary>
        /// Adds the weapon <paramref name="Weapon"/> and saves it. (used for /weapon add) 
        /// </summary>
        /// <param name="Weapon"></param>
        /// <param name="Save"></param>
        public static void AddWeapon(Weapon Weapon, bool Save = true)
        {
            Weapons.Add(Weapon);

            if (Save)
            {
                if (Weapon.FileName != null)
                {
                    Weapon.Save(Weapon.FileName);
                }
                else
                {
                    Weapon.Save();
                }
            }

        }

        private Weapon GetWeaponOfType(string Type, WeaponConfig Config)
        {
            // nonsense
            // when we go to xml this will be a lot better.

            if (Type.CaselessEq(""))
            
        }

        #region temp stuff until WeaponCollection

        public static Weapon GetWeaponWithName(string Name)
        {
            foreach (Weapon Weapon in Weapons.Items) // todo: getenumerator
            {
                if (Weapon.Config.Name.CaselessEq(Name))
                {
                    return Weapon; 
                }
            }

            return null; 
        }

        #endregion
    }
}
