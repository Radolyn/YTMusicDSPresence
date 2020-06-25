#region

using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;

#endregion

namespace RadDiscordProxy
{
    public static class Storage
    {
        public static readonly AppConfiguration Config = AppConfiguration.Initialize<FileManager>("proxy");
    }
}