using Manager;

public struct PublicEvents {
    public delegate void LoadLevelDelegate(int playerHealth);
    public static LoadLevelDelegate LoadNextLevel;

    public delegate void SafeGameDelegate();
    public static SafeGameDelegate SafeGame;

    public delegate void PuzzleIsRightDelegate();
    public static PuzzleIsRightDelegate PuzzleIsRight;

    public delegate void LockMovementDelegate();
    public static LockMovementDelegate LockPlayerMovementToggle;
    
    public delegate void PlayerDelegate(PlayerController player);
    public static PlayerDelegate PlayerNotice;
    
    public delegate void NoCardToPlayDelegate();
    public static NoCardToPlayDelegate NoCardToPlay;
}