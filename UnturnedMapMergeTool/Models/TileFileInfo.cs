using UnturnedMapMergeTool.Extensions;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Models
{
    public class TileFileInfo
    {
        public int OriginalTileX { get; set; }
        public int OriginalTileY { get; set; }

        public string OriginalFileName { get; set; }
        public string FileNameFormat { get; set; }

        public static TileFileInfo FromFileName(string fileName, ETileType tileType)
        {
            string str = fileName.Substring(5, fileName.Length - 5);
            string[] arr = str.Split('_');

            int x = int.Parse(arr[0]);
            int y = int.Parse(arr[1].TrimEnd(".bin"));

            arr[0] = "{0}";
            arr[1] = "{1}";

            string strFormat = string.Join('_', arr);
            if (tileType == ETileType.Hole)
            {
                strFormat += ".bin";
            }
            string fileNameFormat = fileName.Substring(0, 5) + strFormat;

            return new TileFileInfo()
            {
                OriginalTileX = x,
                OriginalTileY = y,
                FileNameFormat = fileNameFormat,
                OriginalFileName = fileName
            };
        }
    }
}
