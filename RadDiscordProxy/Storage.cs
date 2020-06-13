#region

using RadLibrary.Configuration;

#endregion

namespace RadDiscordProxy
{
    public static class Storage
    {
        public static readonly AppConfiguration Config = AppConfiguration.Initialize<FileManager>("proxy");
    }
}