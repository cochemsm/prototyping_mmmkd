public struct PublicEvents {
    public delegate void LoadLevelDelegate();
    public static LoadLevelDelegate LoadNextLevel;

    public delegate void SafeGameDelegate();
    public static SafeGameDelegate SafeGame;
}