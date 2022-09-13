using System.Collections.Generic;
using System.IO;

namespace UnturnedMapMergeTool.Services
{
    public partial class OutputMap
    {
        public string CombinePath(params string[] args)
        {
            List<string> list = new List<string>()
            {
                config.Path
            };
            list.AddRange(args);
            return Path.Combine(list.ToArray());
        }
    }
}
