using System;
using System.Collections.Generic;
using Cards;
using CustomUI;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance {get; private set; }

        public TestingCard test; // TODO: cards need a system
        public RenderTexture map;
        
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

        private void Update() {
            SetMap(map);
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

        private List<string> _categories;
        
        private ListView _category;
        private VisualElement _soundView;
        private VisualElement _screenView;

        private Slider _master;
        private Slider _music;
        private Slider _sfx;

        private DropdownField _resolution;
        private DropdownField _windowMode;
        private Slider _gamma;
        
        private void SetupSettingsMenu() {
            _category = _panels[(int) UIs.SettingsMenu].Q<ListView>("CategorySelector");
            _category.selectionChanged += ChangeCategory;
            _soundView = _panels[(int)UIs.SettingsMenu].Q<VisualElement>("SoundView");
            _screenView = _panels[(int)UIs.SettingsMenu].Q<VisualElement>("ScreenView");
            _master = _panels[(int)UIs.SettingsMenu].Q<Slider>("MasterSound");
            _music = _panels[(int)UIs.SettingsMenu].Q<Slider>("MusicSound");
            _sfx = _panels[(int)UIs.SettingsMenu].Q<Slider>("SFXSound");
            _resolution = _panels[(int)UIs.SettingsMenu].Q<DropdownField>("ResolutionDropDown");
            _windowMode = _panels[(int)UIs.SettingsMenu].Q<DropdownField>("WindowDropDown");
            _gamma = _panels[(int)UIs.SettingsMenu].Q<Slider>("GammaSlider");

            _categories = new List<string>();
            _categories.Add("Audio");
            _categories.Add("Window");
            
            _category.makeItem = () => {
                Label listItem = new Label();
                listItem.AddToClassList("ListItem");
                return listItem;
            };
            _category.bindItem = (elem, i) => ((Label) elem).text = _categories[i];
            _category.itemsSource = _categories;
            
            _resolution.choices.RemoveAt(0);
            foreach (var resolution in Screen.resolutions) {
                _resolution.choices.Add(resolution.width + "x" + resolution.height);
            }
        }

        private void ChangeCategory(IEnumerable<object> selected) {
            if (((string) _category.selectedItem).Equals("Audio")) {
                _soundView.style.display = DisplayStyle.Flex;
                _screenView.style.display = DisplayStyle.None;
            } else {
                _soundView.style.display = DisplayStyle.None;
                _screenView.style.display = DisplayStyle.Flex;
            }
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
        private VisualElement _map;

        private void SetupInGame() {
            _interact = _panels[(int) UIs.InGame].Q<VisualElement>("interactIndicator");
            _map = _panels[(int)UIs.InGame].Q<VisualElement>("Map");
        }
        
        public void ToggleInteractButton() {
            _interact.style.visibility = _interact.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        public void SetMap(RenderTexture image) {
            _map.style.backgroundImage = new StyleBackground(RenderTextureTo2DTexture(image));
        }

        private Texture2D RenderTextureTo2DTexture(RenderTexture rt) {
            var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0, TextureCreationFlags.None);
            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();

            RenderTexture.active = null;

            return texture;
        }

        #endregion
        
        #region Encounter
        
        private LifeMeter _patience;
        private Label _enemyName;
        private Label _enemyInfo;
        private Cardhand _cardhand;
        private Label _speechLabel;

        public UIController(UIs startPanel) {
            this.startPanel = startPanel;
        }

        private void SetupEncounter() {
            _patience = _panels[(int) UIs.Encounter].Q<LifeMeter>("Patience");
            _enemyName = _panels[(int) UIs.Encounter].Q<Label>("EnemyName");
            _enemyInfo = _panels[(int) UIs.Encounter].Q<Label>("EnemyInfo");
            _cardhand = _panels[(int) UIs.Encounter].Q<Cardhand>("Cardhand");
            _speechLabel = _panels[(int) UIs.Encounter].Q<Label>("SpeechLabel");
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

        public void SetSpeech(string text) {
            _speechLabel.text = text;
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