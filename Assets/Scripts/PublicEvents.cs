using Manager;

public struct PublicEvents {
    public delegate void LoadLevelDelegate();
    public static LoadLevelDelegate LoadNextLevel;

    public delegate void SafeGameDelegate();
    public static SafeGameDelegate SafeGame;

    public delegate void CodeIsRightDelegate();
    public static CodeIsRightDelegate CodeIsRight;

    public delegate void LockMovementDelegate();
    public static LockMovementDelegate LockPlayerMovementToggle;
    
    public delegate void PlayerDelegate(PlayerController player);
    public static PlayerDelegate PlayerNotice;
}