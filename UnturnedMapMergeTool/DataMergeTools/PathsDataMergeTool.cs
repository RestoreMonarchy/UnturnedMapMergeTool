using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Models.Contents.Paths;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class PathsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<PathsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 5;
            ushort count = (ushort)Data.Sum(x => x.Content.PathLines.Count);
            PathsDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<PathsDataContent> dataItem in Data)
            {
                foreach (PathLineData pathLine in dataItem.Content.PathLines)
                {
                    PathLineData shiftedPathLine = new()
                    {
                        Material = pathLine.Material,
                        Joints = new(),
                        NewLoop = pathLine.NewLoop
                    };

                    //pathLine.Material = dataItem.CopyMap.ApplyMaterialShift(pathLine.Material);
                    foreach (PathJointData pathJoint in pathLine.Joints)
                    {
                        PathJointData shiftedPathJoint = new()
                        {
                            Tangents = pathJoint.Tangents,
                            IgnoreTerrain = pathJoint.IgnoreTerrain,
                            Offset = pathJoint.Offset,
                            RoadMode = pathJoint.RoadMode,
                            Vertex = pathJoint.Vertex
                        };

                        if (dataItem.CopyMap.IsOriginalPositionBypassed(pathJoint.Vertex))
                        {
                            continue;
                        }

                        dataItem.CopyMap.ApplyPositionShift(shiftedPathJoint.Vertex);
                        shiftedPathLine.Joints.Add(shiftedPathJoint);
                    }


                    shiftedPathLine.Count = (ushort)shiftedPathLine.Joints.Count;

                    if (shiftedPathLine.Count > 0)
                    {
                        content.PathLines.Add(shiftedPathLine);
                    }                    
                }

                content.Count = (ushort)content.PathLines.Count;
            }

            string pathsSavePath = outputMap.CombinePath("Environment/Paths.dat");

            content.SaveToFile(pathsSavePath);

            Log.Information($"Combined and saved {content.PathLines.Count} path lines and {content.PathLines.Sum(x => x.Count)} path joints");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Paths.dat");
            PathsDataContent content = PathsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"paths_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            int pathLinesCount = content.PathLines.Count;
            int pathJoinsCount = content.PathLines.Sum(x => x.Count);

            CopyMapData<PathsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {pathLinesCount} path lines and {pathJoinsCount} path joins");
        }
    }
}
