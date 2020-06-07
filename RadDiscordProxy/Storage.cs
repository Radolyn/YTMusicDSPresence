#region

using RadLibrary.Configuration;
using RadLibrary.Logging;

#endregion

namespace RadDiscordProxy
{
    public static class Storage
    {
        public static readonly Logger NLogger = LoggerUtils.GetLogger("DiscordProxy");
        public static readonly AppConfiguration<FileManager> Config = new AppConfiguration<FileManager>("proxy.conf");
    }
}