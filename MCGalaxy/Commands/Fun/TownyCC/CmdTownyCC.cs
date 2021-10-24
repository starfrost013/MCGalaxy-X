using MCGalaxy.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy.Commands.Fun
{
    /// <summary>
    /// Implements the /tcc command
    /// </summary>
    public class CmdTownyCC : NoRoundsGameCmd
    {
        public override string name { get { return "TownyCC"; } }
        public override string shortcut { get { return "tcc"; } }

        protected override NoRoundsGame Game { get { return TownyCC.Instance; } }

        public override CommandPerm[] ExtraPerms 
        {
            get
            {
                return new CommandPerm[] { new CommandPerm(LevelPermission.Operator, "TCC_Control")};
            }
        }

        public override void Use(Player p, string message, CommandData Data) // there will be a lot of specific faction etc commands
        {
            base.Use(p, message, Data);
        }

        public override void Help(Player p)
        {
            Help(p, null); // todo: enum?
        }

        public override void Help(Player p, string message)
        {
            p.Message("TCC Help Unimplemented");
        }

        protected override void HandleSet(Player p, BaseGame game, string[] args)
        {
            return; // empty for now
        }

    }
}
