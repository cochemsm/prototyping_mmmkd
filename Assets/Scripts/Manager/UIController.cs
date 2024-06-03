using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour {
    public static UIController Instance {get; private set; }
    
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