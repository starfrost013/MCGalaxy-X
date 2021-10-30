using MCGalaxy.Maths; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlockID = System.UInt16;

namespace MCGalaxy.Games
{
    public class PenetrativeGun : Gun
    {
        public override string Name { get { return "Penetrative gun"; } }

        protected override bool OnHitBlock(AmmunitionData args, Vec3U16 pos, BlockID block)
        {
            if (p.level.physics < 2) return true;

            if (!p.level.Props[block].LavaKills) return true;
            // Penetrative gun goes through blocks lava can go through
            p.level.Blockchange(pos.X, pos.Y, pos.Z, Block.Air);
            return false;
        }
    }
}
