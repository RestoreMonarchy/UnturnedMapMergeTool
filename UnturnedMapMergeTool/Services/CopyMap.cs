using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        private readonly CopyMapConfig config;
        private readonly OutputMap outputMap;

        public Guid Identifier { get; private set; }

        public CopyMap(CopyMapConfig config, OutputMap outputMap)
        {
            Identifier = Guid.NewGuid();
            this.config = config;
            this.outputMap = outputMap;
        }

        public CopyMapConfig Config => config;

        private Coordinate StartCoordinate => config.StartCoordinate;

        

        
    }
}
