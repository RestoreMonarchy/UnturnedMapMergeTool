using System.Collections.Generic;
using System.Data;
using UnturnedMapMergeTool.Models.Contents.Paths;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class PathsDataContent
    {
        public PathsDataContent(byte saveDataPathsVersion, ushort count)
        {
            SaveDataPathsVersion = saveDataPathsVersion;
            Count = count;

            PathLines = new();
        }

        public PathsDataContent()
        {

        }

        public byte SaveDataPathsVersion { get; set; }
        public ushort Count { get; set; }

        public List<PathLineData> PathLines { get; set; }


        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);
            river.writeByte(SaveDataPathsVersion);
            river.writeUInt16(Count);

            foreach (PathLineData pathLine in PathLines)
            {
                river.writeUInt16(pathLine.Count);
                river.writeByte(pathLine.Material);
                river.writeBoolean(pathLine.NewLoop);

                foreach (PathJointData pathJoin in pathLine.Joints)
                {
                    river.writeSingleVector3(pathJoin.Vertex);
                    river.writeSingleVector3(pathJoin.Tangents[0]);
                    river.writeSingleVector3(pathJoin.Tangents[1]);
                    river.writeByte(pathJoin.RoadMode);
                    river.writeSingle(pathJoin.Offset);
                    river.writeBoolean(pathJoin.IgnoreTerrain);
                }
            }

            river.closeRiver();
        }

        public static PathsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            PathsDataContent content = new();

            content.SaveDataPathsVersion = river.readByte();
            content.Count = river.readUInt16();
            content.PathLines = new List<PathLineData>();

            for (ushort i = 0; i < content.Count; i++)
            {
                PathLineData pathLine = new();
                content.PathLines.Add(pathLine);

                pathLine.Count = river.readUInt16();
                pathLine.Material = river.readByte();
                pathLine.NewLoop = content.SaveDataPathsVersion > 2 && river.readBoolean();
                pathLine.Joints = new List<PathJointData>();
                
                for (ushort j = 0; j < pathLine.Count; j++)
                {
                    PathJointData pathJoint = new PathJointData();
                    pathLine.Joints.Add(pathJoint);

                    pathJoint.Vertex = river.readSingleVector3();

                    pathJoint.Tangents = new Vector3[2];
                    if (content.SaveDataPathsVersion > 2)
                    {
                        pathJoint.Tangents[0] = river.readSingleVector3();
                        pathJoint.Tangents[1] = river.readSingleVector3();
                    }

                    if (content.SaveDataPathsVersion > 2)
                    {
                        pathJoint.RoadMode = river.readByte();
                    } else
                    {
                        // 2: ERoadMode.FREE
                        pathJoint.RoadMode = 2;
                    }

                    if (content.SaveDataPathsVersion > 4)
                    {
                        pathJoint.Offset = river.readSingle();
                    } else
                    {
                        pathJoint.Offset = 0;
                    }

                    pathJoint.IgnoreTerrain = content.SaveDataPathsVersion > 3 && river.readBoolean();
                }
            }

            return content;
        }
    }
}
