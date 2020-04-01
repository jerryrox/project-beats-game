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

        // =======================================
        // Main root area
        // =======================================
        public const int SplashScreen = 0;
        public const int InitializeScreen = 1;
        
        public const int HomeScreen = 5;
        public const int HomeMenuOverlay = 6;

        public const int SongsScreen = 10;
        public const int PrepareScreen = 11;

        public const int MenuBarOverlay = 100;
        public const int ProfileMenuOverlay = 102;
        public const int MusicMenuOverlay = 103;

        public const int DialogOverlay = 200;
    }
}