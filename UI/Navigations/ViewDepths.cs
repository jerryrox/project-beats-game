namespace PBGame.UI.Navigations
{
    /// <summary>
    /// Pre-defined depth values of views.
    /// </summary>
    public static class ViewDepths {

        // =======================================
        // 3D root area
        // =======================================
        public const int BackgroundOverlay = -1000;
        
        public const int GameScreen = 0;

        // =======================================
        // Main root area
        // =======================================
        public const int SplashScreen = 0;
        public const int InitializeScreen = 1;

        public const int HomeScreen = 5;
        public const int HomeMenuOverlay = 6;
        public const int DownloadScreen = 6;

        public const int SongsScreen = 10;
        public const int PrepareScreen = 11;

        public const int ResultScreen = 20;

        public const int GameLoadOverlay = 50;

        public const int MenuBarOverlay = 100;
        public const int QuickMenuOverlay = 101;
        public const int ProfileMenuOverlay = 102;
        public const int MusicMenuOverlay = 103;
        public const int SettingsMenuOverlay = 104;
        public const int NotificationMenuOverlay = 105;
        public const int ModeMenuOverlay = 106;

        public const int OffsetOverlay = 110;

        public const int DialogOverlay = 200;

        public const int SystemOverlay = 1000;

        public const int QuitOverlay = 1000000;
    }
}