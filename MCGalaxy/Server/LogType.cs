using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCGalaxy
{
    /// <summary>
    /// LogType
    /// 
    /// Enumerates the valid types of logging.
    /// </summary>
    public enum LogType
    {

        /// <summary> Background system activity, such as auto-saving maps, performing GC, etc. </summary>
        BackgroundActivity,

        /// <summary> Normal system activity, such as loading maps, etc. </summary>
        SystemActivity,

        /// <summary> Activity causes by games such as lava survival, TNT wars, etc. </summary>
        GameActivity,

        /// <summary> User activity by players or console, such as connecting, banning players, etc. </summary>
        UserActivity,

        /// <summary> User performs a suspicious activity, such as triggering block spam kick, noclipping in a game, etc. </summary>
        SuspiciousActivity,

        /// <summary> Activity on a relay bot (e.g. IRC or Discord) </summary>
        RelayActivity,

        /// <summary> Warning message, such as failure to save a file. </summary>
        Warning,

        /// <summary> Handled or unhandled exception occurs. </summary>
        Error,

        /// <summary> Command used by a player. </summary>
        CommandUsage,

        /// <summary> Chat globally or only on player's level. </summary>
        PlayerChat,

        /// <summary> Chat relayed from an external communication service (e.g. IRC or Discord)  </summary>
        RelayChat,

        /// <summary> Chat to all players in a particular chatroom, or across all chatrooms. </summary>
        ChatroomChat,

        /// <summary> Chat to all players who have permission to read certain chat group (/opchat, /adminchat). </summary>
        StaffChat,

        /// <summary> Chat from one player to another. </summary>
        PrivateChat,

        /// <summary> Chat to all players of a rank. </summary>
        RankChat,

        /// <summary> Debug messages. </summary>
        Debug,

        /// <summary> Message shown to console. </summary>
        ConsoleMessage,

        /// <summary> TownyCC system messages. /// </summary>
        TCCSystem
    }
}
