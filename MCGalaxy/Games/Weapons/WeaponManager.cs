using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static VolatileArray<Weapon> Weapons { get; set; }
        
        public static void Load()
        {
            Logger.Log(LogType.SystemActivity, "Loading weapons...");
            Weapons = new VolatileArray<Weapon>();

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
    }
}
