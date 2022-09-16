using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Contents.Navigations;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class NavigationsDataContent
    {
        public byte SaveDataNavigationVersion { get; set; }
        public Vector3 ForcedBoundsCenter { get; set; }
        public Vector3 ForcedBoundsSize { get; set; }
        public byte TileXCount { get; set; }
        public byte TileZCount { get; set; }

        public NavmeshTileData[] NavmeshTiles { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataNavigationVersion);
            river.writeSingleVector3(ForcedBoundsCenter);
            river.writeSingleVector3(ForcedBoundsSize);
            river.writeByte(TileXCount);
            river.writeByte(TileZCount);

            for (int i = 0; i < TileZCount; i++)
            {
                for (int j = 0; j < TileXCount; j++)
                {
                    int index = j + i * TileXCount;
                    NavmeshTileData navmeshTile = NavmeshTiles[index];

                    river.writeUInt16(navmeshTile.TrisCount);
                    for (int k = 0; k < navmeshTile.TrisCount; k++)
                    {
                        river.writeUInt16(navmeshTile.Tris[k]);
                    }
                    river.writeUInt16(navmeshTile.VertsCount);
                    for (int l = 0; l < navmeshTile.VertsCount; l++)
                    {
                        NavmeshTileVertData navmeshTileVert = navmeshTile.Verts[l];
                        river.writeInt32(navmeshTileVert.X);
                        river.writeInt32(navmeshTileVert.Y);
                        river.writeInt32(navmeshTileVert.Z);
                    }
                }
            }

            river.closeRiver();
        }

        public static NavigationsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            NavigationsDataContent content = new();

            content.SaveDataNavigationVersion = river.readByte();
            content.ForcedBoundsCenter = river.readSingleVector3();
            content.ForcedBoundsSize = river.readSingleVector3();
            content.TileXCount = river.readByte();
            content.TileZCount = river.readByte();

            content.NavmeshTiles = new NavmeshTileData[content.TileXCount * content.TileZCount];

            for (int i = 0; i < content.TileZCount; i++)
            {
                for (int j = 0; j < content.TileXCount; j++)
                {
                    NavmeshTileData navmeshTile = new();

                    int index = j + i * content.TileXCount;

                    content.NavmeshTiles[index] = navmeshTile;

                    navmeshTile.TrisCount = river.readUInt16();
                    navmeshTile.Tris = new ushort[navmeshTile.TrisCount];
                    for (int k = 0; k < navmeshTile.TrisCount; k++)
                    {
                        navmeshTile.Tris[k] = river.readUInt16(); 
                    }

                    navmeshTile.VertsCount = river.readUInt16();
                    navmeshTile.Verts = new NavmeshTileVertData[navmeshTile.VertsCount];
                    for (int l = 0; l < navmeshTile.VertsCount; l++)
                    {
                        NavmeshTileVertData navmeshTileVert = new();
                        navmeshTileVert.X = river.readInt32();
                        navmeshTileVert.Y = river.readInt32();
                        navmeshTileVert.Z = river.readInt32();

                        navmeshTile.Verts[l] = navmeshTileVert;
                    }
                }
            }

            river.closeRiver();
            return content;
        }
    }
}
