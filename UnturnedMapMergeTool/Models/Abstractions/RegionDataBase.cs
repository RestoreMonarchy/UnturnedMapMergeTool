using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Abstractions
{
    public abstract class RegionDataBase<T> where T : class
    {
        public ushort Count { get; set; }
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public List<T> Items { get; set; }


    }
}
