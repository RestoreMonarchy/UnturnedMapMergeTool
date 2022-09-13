using System.IO;
using System.Linq;

namespace UnturnedMapMergeTool.Models
{
    public class TileFileInfo
    {
        public int OriginalTileX { get; set; }
        public int OriginalTileY { get; set; }

        public string OriginalFileName { get; set; }
        public string FileNameFormat { get; set; }

        public string FileName => string.Format(FileNameFormat, OriginalTileX, OriginalTileY);

        public static TileFileInfo FromFileName(string fileName)
        {
            string str = fileName.Substring(5, fileName.Length - 5);
            string[] arr = str.Split('_');

            int horizontal = int.Parse(arr[0]);
            int vertical = int.Parse(arr[1]);

            arr[0] = "{0}";
            arr[1] = "{1}";

            string strFormat = string.Join('_', arr);
            string fileNameFormat = fileName.Substring(0, 5) + strFormat;

            return new TileFileInfo()
            {
                OriginalTileX = horizontal,
                OriginalTileY = vertical,
                FileNameFormat = fileNameFormat,
                OriginalFileName = fileName
            };
        }
    }
}
