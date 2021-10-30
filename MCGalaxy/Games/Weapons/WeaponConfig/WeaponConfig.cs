using MCGalaxy.Config; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary>
    /// WeaponConfig
    /// 
    /// Defines the weapon configuration. (unfinished - more will be added)
    /// </summary>
    public class WeaponConfig
    {
        public ConfigElement[] Config;

        // pretty sure due to the way its loaded all config stuff has to be fields

        /// <summary>
        /// The name of this weapon.
        /// </summary>
        [ConfigString("Name", "Main", "Weapon Name Undefined - Please Set")]
        public string Name = "Weapon Name Undefined - Please Set";

        /// <summary>
        /// The description of this weapon.
        /// </summary>
        [ConfigString("Description", "Main", "")]
        public string Description = "";

        /// <summary>
        /// The maximum damage of this weapon.
        /// 
        /// Health is shared across all blocks and entities.
        /// </summary>
        [ConfigInt("MaximumDamage", "Main", 20)]
        public int MaximumDamage = 20;

        // A formula will be used in order to usually award "close" to the maximum damage

        /// <summary>
        /// Minimum damage percentage.
        /// </summary>
        [ConfigDouble("MinimumDamagePercentage", "Main", 0.7)]
        public double MinimumDamagePercentage = 0.7;

        /// <summary>
        /// Maximum damage percentage.
        /// </summary>
        [ConfigDouble("MaximumDamagePercentage", "Main", 1.3)]
        public double MaximumDamagePercentage = 1.3;

        //minicrit?

        /// <summary>
        /// Determines if critical hits are allowed.
        /// </summary>
        [ConfigBool("AllowCriticalHit", "Main", false)]
        public bool AllowCriticalHit = false;

        /// <summary>
        /// Target damage multiplier used for critical hits if enabled
        /// </summary>
        [ConfigDouble("CriticalHitDamageMultiplier", "Main", 1.6)]
        public double CriticalHitDamageMultiplier = 1.6;

        /// <summary>
        /// TF2 fans beware :). If false, headshot = critical hit.
        /// </summary>
        [ConfigBool("RandomCriticalHits", "Main", false)]
        public bool RandomCriticalHits = false;

        /// <summary>
        /// If <see cref="RandomCriticalHits"/> is true, the chance of a random critical hit.
        /// </summary>
        [ConfigDouble("RandomCriticalHitChance", "Main", 0.03, 0)]
        public double RandomCriticalHitChance = 0.03;

        /// <summary>
        /// Whether this weapon allows damage variance - if false, the same damage will be dealt each time.
        /// </summary>
        [ConfigBool("AllowDamageVariance", "Main", true)]
        public bool AllowDamageVariance = true;

        /// <summary>
        /// Average damage variance of this weapon - a gradient is used
        /// </summary>
        [ConfigDouble("AverageDamageVariance", "Main", 0.05)]
        public double AverageDamageVariance = 0.05;

        /// <summary>
        /// Whether this weapon uses automatic reloading.
        /// </summary>
        [ConfigBool("AutomaticReloading", "Main", true)]
        public bool AutomaticReloading = true;

        /// <summary>
        /// Whether this weapon has infinite ammo.
        /// </summary>
        [ConfigBool("InfiniteAmmo", "Main", false)]
        public bool InfiniteAmmo = false;

        /// <summary>
        /// The clip size of this weapon
        /// </summary>
        [ConfigInt("ClipSize", "Main", 32, 0)]
        public int ClipSize = 32;

        /// <summary>
        /// The reload time of this weapon.
        /// </summary>
        [ConfigDouble("ReloadTime", "Main", 1.0, 0)]
        public double ReloadTime = 1.0;

        /// <summary>
        /// The type of this weapon - see <see cref="WeaponType"/>
        /// </summary>
        [ConfigEnum("Type", "Main", WeaponType.Normal, typeof(WeaponType))]
        public WeaponType Type = WeaponType.Normal;

        /// <summary>
        /// Average recoil of this weapon (if not melee)
        /// </summary>
        [ConfigDouble("RecoilBlocks", "Main", 0.5, 0)]
        public double RecoilBlocks = 0.5;

        /// <summary>
        /// Time between shots.
        /// </summary>
        [ConfigDouble("RecoilTime", "Main", 0, 0)]
        public double RecoilTime = 0;

        /// <summary>
        /// The number of uses per second.
        /// </summary>
        [ConfigInt("FireSpeed", "Main", 1)]
        public int FireSpeed = 1;

        /// <summary>
        /// The ammunition block fired by this weapon. 
        /// </summary>
        [ConfigString("Ammunition", "Main", "Bullet")]
        public string Ammunition = "Bullet";

        /// <summary>
        /// Can this weapon fail and blow up in your face, killing you immediately?
        /// </summary>
        [ConfigBool("CanFail", "Fun", false)]
        public bool CanFail = false;

        /// <summary>
        /// Chance of this weapon failing and blowing you up.
        /// </summary>
        [ConfigDouble("FailureChance", "Fun", 0.1)]
        public double FailureChance = 0.1;
   
        /// <summary>
        /// Explosion radius - ignored if <see cref="Type"/> is not <see cref="WeaponType.Explode"/>.
        /// </summary>
        [ConfigDouble("ExplosionRadius", "Fun", 5)]
        public double ExplosionRadius = 5;

        /// <summary>
        /// If <see cref="CanFail"/> is true, this weapon will never work. If it is false, this will act like a suicide weapon.
        /// </summary>
        [ConfigBool("SuicideWeapon", "Fun?", false)]
        public bool SuicideWeapon = false;


        // more to come!

        #region Methods

        public void Load(string FileName = null)
        {
            if (Config == null) Config = ConfigElement.GetAll(typeof(WeaponConfig));

            if (FileName == null)
            {
                PropertiesFile.Read(WeaponInfo.GetPropertiesPath(Name), ProcessPropertyLine);
            }
            else
            {
                PropertiesFile.Read(WeaponInfo.GetPropertiesPath(FileName), ProcessPropertyLine);
            }
           
        }

        private void ProcessPropertyLine(string Key, string Value) => ConfigElement.Parse(Config, this, Key, Value);
        public void Save(string FileName = null)
        {
            if (Config == null) Config = ConfigElement.GetAll(typeof(WeaponConfig));

            if (FileName == null)
            {
                FileName = WeaponInfo.GetPropertiesPath(Name);
            }
            else
            {
                FileName = WeaponInfo.GetPropertiesPath(FileName); 
            }

            using (StreamWriter SW = new StreamWriter(new FileStream(FileName, FileMode.OpenOrCreate)))
            {
                SW.WriteLine($"# Weapon config for {Name} saved at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                ConfigElement.Serialise(Config, SW, this);
            }
        }

        #endregion
    }
}
