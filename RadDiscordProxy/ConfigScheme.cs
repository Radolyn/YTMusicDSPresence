#region

using RadLibrary.Configuration.Scheme;

#endregion

namespace RadDiscordProxy
{
    public class ConfigScheme
    {
        [SchemeParameter(Comment = "The application id.\nCan be found on Discord Developer Portal.")]
        public ulong ApplicationId = 718862357282947144;

        [SchemeParameter(Comment =
            "The port which on server will be listening for messages from TamperMonkey script.\nDon' forget to change it in the script too.")]
        public ushort Port = 1339;

        [SchemeParameter(Comment = "Timeout to remove Discord Rich Presence (e.g. you closed browser)")]
        public uint Timeout = 5000;

        [SchemeParameter(Comment = "The large image key. Upload it in application settings.")]
        public string LargeImageKey = "1200px-youtube_music_logo_svg";

        [SchemeParameter(Comment = "The large image text.")]
        public string LargeImageText = "YouTube Music";

        [SchemeParameter(Comment = "The paused icon. Upload it in application settings.")]
        public string Paused = "pause_v2";

        [SchemeParameter(Comment = "The playing text.")]
        public string Playing = "play";

        [SchemeParameter(Comment = "The paused text.")]
        public string PausedText = "Простаивает";

        [SchemeParameter(Comment = "The paused text.")]
        public string PlayingText = "Слушает";

        [SchemeParameter(Comment = "Enable console's beeps on join request?")]
        public bool EnableJoinRequestSound = true;

        [SchemeParameter(Comment = "Enable auto url opening on join?")]
        public bool EnableAutoUrlOpen = true;

        [SchemeParameter(Comment = "Enable invite feature?")]
        public bool EnableInviteFeature = true;

        [SchemeParameter(Comment = "Hide console?")]
        public bool HideConsole = false;
    }
}