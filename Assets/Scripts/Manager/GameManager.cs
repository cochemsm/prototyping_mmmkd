using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set; }
     
    private void Awake() {
        if (Instance is not null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    private void OnDestroy() {
        if (Instance == this) Instance = null;
    }
}