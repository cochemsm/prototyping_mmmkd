using Cards;
using CustomUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance {get; private set; }

        [SerializeField] private VisualTreeAsset[] uis;
        private const int ingame = 0;
        private const int encounter = 1;
        
        public Card test;
        
        private UIDocument ui;
        private VisualElement body;
    
        private ProgressBar systemPower;
        private Label systemPowerLabel;
        private LifeMeter patience;
        private Label enemyName;
        private Label enemyInfo;
        private Cardhand cardhand;

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

            systemPower = body.Q<ProgressBar>("SystemPower");
            systemPowerLabel = body.Q<Label>("SystemPowerLabel");
            patience = body.Q<LifeMeter>("Patience");
            enemyName = body.Q<Label>("EnemyName");
            enemyInfo = body.Q<Label>("EnemyInfo");
            cardhand = body.Q<Cardhand>("Cardhand");
            
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
            AddCardToHand(test);
        }
    
        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }

        public void AddCardToHand(Card newCard) {
            cardhand.AddCard(newCard, body);
        }

        public void ToggleInteractButton() {
            interact.style.display = interact.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}