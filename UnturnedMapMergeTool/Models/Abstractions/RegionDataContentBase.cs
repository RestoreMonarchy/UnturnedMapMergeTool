using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Abstractions
{
    public abstract class RegionDataContentBase<T, T2> where T : RegionDataBase<T2> where T2 : class
    {
        public List<T> Regions { get; set; }

        protected void LoadRegions()
        {

        }
    }
}
