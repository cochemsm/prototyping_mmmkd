using UnityEngine;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance {get; private set; }

        private UIDocument ui;
        private VisualElement body;
    
        private ProgressBar systemPower;
        private Label systemPowerLabel;
        // private Battery patience;
        private Label enemyName;
        private Label enemyInfo;
        // private Cardhand cardhand;
    
        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            ui = GetComponent<UIDocument>();
            body = ui.rootVisualElement;

            systemPower = body.Q<ProgressBar>("SystemPower");
            systemPowerLabel = body.Q<Label>("SystemPowerLabel");

            enemyName = body.Q<Label>("EnemyName");
            enemyInfo = body.Q<Label>("EnemyInfo");
        
            body.Q<VisualElement>("cardTemplate").AddManipulator(new DragAndDrop(body));
        }
    
        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }
    }
}