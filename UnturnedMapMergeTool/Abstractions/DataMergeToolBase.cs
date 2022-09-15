using System;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.Abstractions
{
    public abstract class DataMergeToolBase : IDataMergeTool
    {
        public abstract void CombineAndSaveData(OutputMap outputMap);

        public abstract void ReadData(CopyMap copyMap);
    }
}
