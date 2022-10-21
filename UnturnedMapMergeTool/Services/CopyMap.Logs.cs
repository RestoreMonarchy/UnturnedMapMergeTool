using Serilog;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        private void LogTile(ETileType tileType, string message)
        {
            LogInformation($"[{tileType}] {message}");
        }

        private void LogInformation(string message)
        {
            Log.Information($"[{config.Name}]: {message}");
        }
    }
}
