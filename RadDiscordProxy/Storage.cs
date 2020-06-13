#region

using RadLibrary.Configuration;
using RadLibrary.Logging;

#endregion

namespace RadDiscordProxy
{
    public static class Storage
    {
        public static readonly AppConfiguration Config = AppConfiguration.Initialize<FileManager>("proxy");
    }
}