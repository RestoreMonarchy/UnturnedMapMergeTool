using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.FlagsData
{
    public class FlagDataData
    {
        public string DifficultyGuid { get; set; }
        public byte MaxZombies { get; set; }
        public bool SpawnZombies { get; set; }
        public bool HyperAgro { get; set; }
        public int MaxBossZombies { get; set; }
    }
}
