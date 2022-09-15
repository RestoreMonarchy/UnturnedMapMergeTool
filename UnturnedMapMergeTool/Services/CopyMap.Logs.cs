using System;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        private void LogTile(ETileType tileType, string message)
        {
            Log($"[{tileType}] {message}");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{config.Name}]: {message}");
        }
    }
}
