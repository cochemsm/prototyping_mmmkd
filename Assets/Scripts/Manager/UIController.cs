using Cards;
using CustomUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance {get; private set; }

        [SerializeField] private VisualTreeAsset[] uis;
        public enum UIs {
            inGame,
            encounter
        }
        
        private UIDocument ui;
        private VisualElement body;
        
        public Card test;
        
        // encounter
        private ProgressBar systemPower;
        private Label systemPowerLabel;
        private LifeMeter patience;
        private Label enemyName;
        private Label enemyInfo;
        private Cardhand cardhand;
        
        // ingame
        private VisualElement interact;
    
        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            ui = GetComponent<UIDocument>();
            body = ui.rootVisualElement;
            
            // encounter
            systemPower = body.Q<ProgressBar>("SystemPower");
            systemPowerLabel = body.Q<Label>("SystemPowerLabel");
            patience = body.Q<LifeMeter>("Patience");
            enemyName = body.Q<Label>("EnemyName");
            enemyInfo = body.Q<Label>("EnemyInfo");
            cardhand = body.Q<Cardhand>("Cardhand");
            
            // ingame
            interact = body.Q<VisualElement>("interactIndicator");
        }
    
        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }
        
        public void ChangeUI(UIs newUI) {
            ui.visualTreeAsset = uis[(int) newUI];
        }

        public void AddCardToHand(Card newCard) {
            cardhand.AddCard(newCard, body);
        }

        public void ToggleInteractButton() {
            interact.style.visibility = interact.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
    }
}