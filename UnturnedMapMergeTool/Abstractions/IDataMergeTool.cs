using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.Abstractions
{
    public interface IDataMergeTool
    {
        void ReadData(CopyMap copyMap);
        void CombineAndSaveData(OutputMap outputMap);
    }
}
