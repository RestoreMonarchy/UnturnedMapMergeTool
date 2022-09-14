using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class CSteamID
    {
        public ulong m_SteamID { get; set; }

        public CSteamID(ulong steamID)
        {
            m_SteamID = steamID;
        }
    }
}
