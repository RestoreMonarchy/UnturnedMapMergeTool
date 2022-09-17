using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.Faunas
{
    public class AnimalTierData
    {
        public string Name { get; set; }
        public float Chance { get; set; }
        public byte SpawnsCount { get; set; }
        public List<AnimalSpawnData> Spawns { get; set; }
    }
}
