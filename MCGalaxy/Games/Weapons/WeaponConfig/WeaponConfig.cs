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
        public string Name;

        /// <summary>
        /// The description of this weapon.
        /// </summary>
        [ConfigString("Description", "Main", "")]
        public string Description;

        /// <summary>
        /// The maximum damage of this weapon.
        /// </summary>
        [ConfigInt("Name", "Main", 20)]
        public int MaximumDamage;

        // A formula will be used in order to usually award "close" to the maximum damage

        /// <summary>
        /// Minimum damage percentage.
        /// </summary>
        [ConfigDouble("MinimumDamagePercentage", "Main", 0.7 )]
        public double MinimumDamagePercentage;

        /// <summary>
        /// Maximum damage percentage.
        /// </summary>
        [ConfigDouble("MaximumDamagePercentage", "Main", 1.3 )]
        public double MaximumDamagePercentage;

        //minicrit?

        /// <summary>
        /// Determines if critical hits are allowed.
        /// </summary>
        [ConfigBool("AllowCriticalHit", "Main", false)]
        public bool AllowCriticalHit;

        /// <summary>
        /// Target damage multiplier used for critical hits if enabled
        /// </summary>
        [ConfigDouble("CriticalHitDamageMultiplier", "Main", 1.6)]
        public double CriticalHitDamageMultipliere;

        /// <summary>
        /// TF2 fans beware :). If false, headshot = critical hit.
        /// </summary>
        [ConfigBool("RandomCriticalHits", "Main", false)]
        public bool RandomCriticalHits;

        /// <summary>
        /// If <see cref="RandomCriticalHits"/> is true, the chance of a random critical hit.
        /// </summary>
        [ConfigDouble("RandomCriticalHitChance", "Main", 0.03, 0)]
        public double RandomCriticalHitChance;

        /// <summary>
        /// Whether this weapon allows damage variance - if false, the same damage will be dealt each time.
        /// </summary>
        [ConfigBool("AllowDamageVariance", "Main", true)]
        public bool AllowDamageVariance;

        /// <summary>
        /// Average damage variance of this weapon - a gradient is used
        /// </summary>
        [ConfigDouble("AverageDamageVariance", "Main", 0.05)]
        public double AverageDamageVariance;

        /// <summary>
        /// Whether this weapon uses automatic reloading.
        /// </summary>
        [ConfigBool("AutomaticReloading", "Main", true)]
        public bool AutomaticReloading;

        /// <summary>
        /// Whether this weapon has infinite ammo.
        /// </summary>
        [ConfigBool("InfiniteAmmo", "Main", false)]
        public bool InfiniteAmmo;

        /// <summary>
        /// The clip size of this weapon
        /// </summary>
        [ConfigInt("ClipSize", "Main", 32, 0)]
        public int ClipSize;

        /// <summary>
        /// The reload time of this weapon.
        /// </summary>
        [ConfigDouble("ReloadTime", "Main", 1.0, 0)]
        public double ReloadTime;

        /// <summary>
        /// The type of this weapon - see <see cref="WeaponType"/>
        /// </summary>
        [ConfigEnum("Type", "Main", WeaponType.Normal, typeof(WeaponType))]
        public WeaponType Type;

        /// <summary>
        /// Average recoil of this weapon (if not melee)
        /// </summary>
        [ConfigDouble("RecoilBlocks", "Main", 0.5, 0)]
        public double RecoilBlocks;

        /// <summary>
        /// The number of uses per second.
        /// </summary>
        [ConfigInt("FireSpeed", "Main", 1)]
        public int FireSpeed;

        /// <summary>
        /// Can this weapon fail and blow up in your face, killing you immediately?
        /// </summary>
        [ConfigBool("CanFail", "Fun", false)]
        public bool CanFail;

        /// <summary>
        /// Chance of this weapon failing and blowing you up.
        /// </summary>
        [ConfigDouble("FailureChance", "Fun", 0.1)]
        public double FailureChance;
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
        public void Save()
        {
            if (Config == null) Config = ConfigElement.GetAll(typeof(WeaponConfig));

            using (StreamWriter SW = new StreamWriter(new FileStream(WeaponInfo.GetPropertiesPath(Name), FileMode.OpenOrCreate)))
            {
                SW.WriteLine($"# Weapon config for {Name} saved at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                ConfigElement.Serialise(Config, SW, this);
            }
        }

        #endregion
    }
}
