using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Zombies
{
    public class ZombieTableData
    {
        public Guid TableUniqueId { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public bool IsMega { get; set; }
        public ushort Health { get; set; }
        public byte Damage { get; set; }
        public byte LootIndex { get; set; }
        public ushort LootId { get; set; }
        public uint XP { get; set; }
        public float Regen { get; set; }
        public string DifficultyGuid { get; set; }

        public byte SlotsCount { get; set; }
        public ZombieSlotData[] Slots { get; set; }
    }
}
