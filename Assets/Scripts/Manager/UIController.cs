using System.Collections.Generic;
using System.Globalization;
using Cards;
using CustomUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance {get; private set; }

        public TestingCard test;
        
        private UIDocument _ui;
        private VisualElement _root;

        private List<VisualElement> _panels = new List<VisualElement>();
        public enum UIs {
            MainMenu,
            SettingsMenu,
            PauseMenu,
            InGame,
            Encounter,
            HoloOverlay,
            GameOver
        }

        [SerializeField] private UIs startPanel;
        public UIs CurrentPanel { get; private set; }
        
        #region Unity Functions

        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            _ui = GetComponent<UIDocument>();
            _root = _ui.rootVisualElement;
            
            SetupPanels();
            SetupMainMenu();
            SetupSettingsMenu();
            SetupPauseMenu();
            SetupHoloOverlay();
            SetupEncounter();
            SetupInGame();
            SetupGameOver();
            
            ChangePanel(startPanel);
            
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

        #endregion

        #region Panels

        private void SetupPanels() {
            _panels = _root.Query<VisualElement>(className: "panel").ToList();
        }

        public void ChangePanel(UIs newUI) {
            switch (CurrentPanel) {
                case UIs.InGame:
                case UIs.Encounter: 
                    _panels[(int) UIs.HoloOverlay].style.display = DisplayStyle.None;
                    break;
            }

            _panels[(int)CurrentPanel].style.display = DisplayStyle.None;
            
            switch (newUI) {
                case UIs.InGame:
                case UIs.Encounter: 
                    _panels[(int) UIs.HoloOverlay].style.display = DisplayStyle.Flex;
                    break;
                case UIs.GameOver:
                    // TODO: load last scene
                    break;
            } 
            
            _panels[(int) newUI].style.display = DisplayStyle.Flex;
            CurrentPanel = newUI;
        }

        #endregion
        
        #region Main Menu

        private void SetupMainMenu() {
            _panels[(int) UIs.MainMenu].Q<Button>("ContinueButton").clicked += Continue;
            _panels[(int) UIs.MainMenu].Q<Button>("PlayButton").clicked += Play;
            _panels[(int) UIs.MainMenu].Q<Button>("SettingsButton").clicked += Settings;
            _panels[(int) UIs.MainMenu].Q<Button>("QuitButton").clicked += Quit;
        }

        private void Continue() {
            // TODO: here safed things need to be loaded and started
        }

        private void Play() {
            if (GameManager.Instance is not null) GameManager.Instance.LoadNextLevel();
            ChangePanel(UIs.InGame);
        }

        private void Settings() {
            ChangePanel(UIs.SettingsMenu);
        }

        private void Quit() {
            Application.Quit();
        }

        #endregion
        
        #region Settings Menu

        private ListView _category;
        private ScrollView _soundView;
        private ScrollView _screenView;

        private Slider _master;
        private Slider _music;
        private Slider _sfx;

        private DropdownField _resolution;
        private DropdownField _windowMode;
        private Slider _gamma;
        
        private void SetupSettingsMenu() {
            _panels[(int) UIs.SettingsMenu].Q<ListView>("CategorySelector").selectionChanged += ChangeCategory;
            _soundView = _panels[(int)UIs.SettingsMenu].Q<ScrollView>("SoundView");
            _screenView = _panels[(int)UIs.SettingsMenu].Q<ScrollView>("ScreenView");
            _master = _panels[(int)UIs.SettingsMenu].Q<Slider>("MasterSound");
            _music = _panels[(int)UIs.SettingsMenu].Q<Slider>("MusicSound");
            _sfx = _panels[(int)UIs.SettingsMenu].Q<Slider>("SFXSound");
            _resolution = _panels[(int)UIs.SettingsMenu].Q<DropdownField>("ResolutionDropDown");
            _windowMode = _panels[(int)UIs.SettingsMenu].Q<DropdownField>("WindowDropDown");
            _gamma = _panels[(int)UIs.SettingsMenu].Q<Slider>("GammaSlider");
        }

        private void ChangeCategory(IEnumerable<object> category) {
            
        }

        #endregion

        #region Pause Menu

        private void SetupPauseMenu() {
            _panels[(int) UIs.PauseMenu].Q<Button>("ResumeButton").clicked += Resume;
            _panels[(int) UIs.PauseMenu].Q<Button>("SettingsButton").clicked += Settings;
            _panels[(int) UIs.PauseMenu].Q<Button>("ExitButton").clicked += Exit;
        }

        private void Resume() {
            if (GameManager.Instance is not null) GameManager.Instance.PauseToggle();
        }
        
        // Settings function identical for Main Menu
        
        private void Exit() {
            // TODO: game manager needs to load specific scenes
        }

        #endregion
        
        #region Holo Overlay
        
        private ProgressBar _systemPower;
        private Label _systemPowerLabel;
        
        private void SetupHoloOverlay() {
            _systemPower = _panels[(int)UIs.HoloOverlay].Q<ProgressBar>("SystemPower");
            _systemPowerLabel = _panels[(int)UIs.HoloOverlay].Q<Label>("SystemPowerLabel");
        }

        public void SetSystemPower(int value) {
            _systemPower.value = value;
            _systemPowerLabel.text = "" + value + "%";
        }

        #endregion
        
        #region In Game

        private VisualElement _interact;

        private void SetupInGame() {
            _interact = _root.Q<VisualElement>("interactIndicator");
        }
        
        public void ToggleInteractButton() {
            _interact.style.visibility = _interact.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        #endregion
        
        #region Encounter
        
        private LifeMeter _patience;
        private Label _enemyName;
        private Label _enemyInfo;
        private Cardhand _cardhand;

        public UIController(UIs startPanel) {
            this.startPanel = startPanel;
        }

        private void SetupEncounter() {
            _patience = _root.Q<LifeMeter>("Patience");
            _enemyName = _root.Q<Label>("EnemyName");
            _enemyInfo = _root.Q<Label>("EnemyInfo");
            _cardhand = _root.Q<Cardhand>("Cardhand");
        }
        
        public void AddCardToHand(Card newCard) {
            _cardhand.AddCard(newCard, _root);
        }

        public void SetPatience() {
            // TODO: LifeMeter does not have a field for settings patience
        }

        public void SetEnemyInfo() {
            // TODO: Implement something to save enemy data to read out here
        }
        
        #endregion
        
        #region Game Over

        private Label _deathText;
        
        private void SetupGameOver() {
            _deathText = _panels[(int)UIs.GameOver].Q<Label>("DeathText");
            _panels[(int)UIs.GameOver].Q<Button>("CheckpointButton").clicked += LoadCheckpoint;
            _panels[(int)UIs.GameOver].Q<Button>("RestartButton").clicked += Restart;
            _panels[(int)UIs.GameOver].Q<Button>("ExitButton").clicked += Exit;
        }

        private void LoadCheckpoint() {
            // TODO: load safe point and start from there
        }

        private void Restart() {
            // TODO: load first scene
        }
        
        // Exit function identical to pause menu

        #endregion
    }
}