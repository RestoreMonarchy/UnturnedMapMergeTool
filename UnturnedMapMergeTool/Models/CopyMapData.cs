using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.Models
{
    public class CopyMapData<T> where T : class
    {
        public CopyMap CopyMap { get; set; }
        public T Content { get; set; }
    }
}
