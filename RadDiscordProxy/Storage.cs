#region

using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;
using RadLibrary.Configuration.Managers.IniManager;

#endregion

namespace RadDiscordProxy
{
    public static class Storage
    {
        public static readonly IniManager Config = new IniManager("proxy.conf");
    }
}
