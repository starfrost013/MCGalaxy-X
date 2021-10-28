using MCGalaxy;
using MCGalaxy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlockID = System.UInt16;

namespace MCGalaxy.Games
{
    public class AmmunitionData
    {
        public BlockID block;
        public Vec3U16 start;
        public Vec3F32 dir;
        public bool moving = true;

        // positions of all currently visible "trailing" blocks
        public List<Vec3U16> visible = new List<Vec3U16>();
        // position of all blocks this ammunition has touched/gone through
        public List<Vec3U16> all = new List<Vec3U16>();
        public int iterations;

        public Vec3U16 PosAt(int i)
        {
            Vec3U16 target;
            target.X = (ushort)Math.Round(start.X + (double)(dir.X * i));
            target.Y = (ushort)Math.Round(start.Y + (double)(dir.Y * i));
            target.Z = (ushort)Math.Round(start.Z + (double)(dir.Z * i));
            return target;
        }

        public void DoTeleport(Player p)
        {
            int i = visible.Count - 3;

            if (i >= 0 && i < visible.Count)
            {
                Vec3U16 coords = visible[i];
                Position pos = new Position(coords.X * 32, coords.Y * 32 + 32, coords.Z * 32);
                p.SendPosition(pos, p.Rot);
            }
        }
    }

}
