using MCGalaxy.Maths;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Games
{
    /// <summary> Manages the glass box around the player. Adjusts based on where player is looking. </summary>
    internal sealed class AimBox
    {

        Player player;
        List<Vec3U16> lastGlass = new List<Vec3U16>();
        List<Vec3U16> curGlass = new List<Vec3U16>();

        public void Hook(Player p)
        {
            player = p;
            SchedulerTask task = new SchedulerTask(AimCallback, null, TimeSpan.Zero, true);
            p.CriticalTasks.Add(task);
        }

        void AimCallback(SchedulerTask task)
        {
            Player p = player;
            if (p.aiming) { Update(); return; }

            foreach (Vec3U16 pos in lastGlass)
            {
                if (!p.level.IsValidPos(pos)) continue;
                p.RevertBlock(pos.X, pos.Y, pos.Z);
            }
            task.Repeating = false;
        }

        void Update()
        {
            Player p = player;
            Vec3F32 dir = DirUtils.GetDirVector(p.Rot.RotY, p.Rot.HeadX);
            ushort x = (ushort)Math.Round(p.Pos.BlockX + dir.X * 3);
            ushort y = (ushort)Math.Round(p.Pos.BlockY + dir.Y * 3);
            ushort z = (ushort)Math.Round(p.Pos.BlockZ + dir.Z * 3);

            int dx = Math.Sign(dir.X) >= 0 ? 1 : -1, dz = Math.Sign(dir.Z) >= 0 ? 1 : -1;
            Check(p.level, x, y, z);
            Check(p.level, x + dx, y, z);
            Check(p.level, x, y, z + dz);
            Check(p.level, x + dx, y, z + dz);

            // Revert all glass blocks now not in the ray from the player's direction
            for (int i = 0; i < lastGlass.Count; i++)
            {
                Vec3U16 pos = lastGlass[i];
                if (curGlass.Contains(pos)) continue;

                if (p.level.IsValidPos(pos))
                    p.RevertBlock(pos.X, pos.Y, pos.Z);
                lastGlass.RemoveAt(i); i--;
            }

            // Place the new glass blocks that are in the ray from the player's direction
            foreach (Vec3U16 pos in curGlass)
            {
                if (lastGlass.Contains(pos)) continue;
                lastGlass.Add(pos);
                p.SendBlockchange(pos.X, pos.Y, pos.Z, Block.Glass);
            }
            curGlass.Clear();
        }

        void Check(Level lvl, int x, int y, int z)
        {
            Vec3U16 pos = new Vec3U16((ushort)x, (ushort)(y - 1), (ushort)z);
            if (lvl.IsAirAt(pos.X, pos.Y, pos.Z)) curGlass.Add(pos);

            pos.Y++;
            if (lvl.IsAirAt(pos.X, pos.Y, pos.Z)) curGlass.Add(pos);
        }

    }
}
