using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{

    /// <summary>
    /// WeaponType
    /// 
    /// Defines the weapon types
    /// </summary>
    public enum WeaponType { 
    
    /// <summary>
    /// Invalid weapon type
    /// </summary>
    Invalid,
    
    /// <summary>
    /// Normal weapon type
    /// </summary>
    Normal, 
    
    /// <summary>
    /// Destroy weapon type
    /// </summary>
    Destroy, 
    
    /// <summary>
    /// Teleport weapon type
    /// </summary>
    Teleport, 

    /// <summary>
    /// Heat seeking teleport weapon type (e.g. heatseeking missile)
    /// </summary>
    Teleport_HeatSeeking,
    
    /// <summary>
    /// Exploding weapon type
    /// </summary>
    Explode, 
    
    /// <summary>
    /// Laser weapon type
    /// </summary>
    Laser,
    
    /// <summary>
    /// Melee weapon type (e.g. hands)
    /// </summary>
    Melee, 
   
    /// <summary>
    /// Ranged weapon type (e.g. rocket launcher)
    /// </summary>
    Ranged

    // more coming soon!
    };

}
