using System;
using System.Collections;
using System.Collections.Generic;
using CustomUI;
using Objects.Cards;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace Manager {
    public class UIController : MonoBehaviour {
        public static UIController Instance { get; private set; }
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
            KeyPad,
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
            SetupKeyPad();
            SetupInGame();
            SetupGameOver();
            
            ChangePanel(startPanel);
        }
    
        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }

        private void FixedUpdate() {
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
                case UIs.SettingsMenu:
                    newUI = _lastPanelBeforeSettings;
                    break;
                case UIs.KeyPad:
                    PublicEvents.LockPlayerMovementToggle?.Invoke();
                    break;
            }

            _panels[(int)CurrentPanel].style.display = DisplayStyle.None;
            
            switch (newUI) {
                case UIs.InGame:
                    _panels[(int) UIs.HoloOverlay].style.display = DisplayStyle.Flex;
                    break;
                case UIs.Encounter: 
                    _panels[(int) UIs.HoloOverlay].style.display = DisplayStyle.Flex;
                    _patience.ActiveMeterFields = int.MaxValue;
                    break;
                case UIs.SettingsMenu:
                    _lastPanelBeforeSettings = CurrentPanel;
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
            // TODO: here saved things need to be loaded and started
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

        private UIs _lastPanelBeforeSettings;
        
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

        private LiftGammaGain _gammaSetting;
        
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
            _panels[(int)UIs.SettingsMenu].Q<Button>("Back").clicked += Back;
            
            _categories = new List<string>();
            _categories.Add("Audio");
            _categories.Add("Video");
            
            _category.makeItem = () => {
                Label listItem = new Label();
                listItem.AddToClassList("ListItem");
                return listItem;
            };
            _category.bindItem = (elem, i) => ((Label) elem).text = _categories[i];
            _category.itemsSource = _categories;
            _category.SetSelection(0);
            
            _master.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.MasterVolume, evt.newValue));
            _music.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.MusicVolume, evt.newValue));
            _sfx.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.SfxVolume, evt.newValue));
            
            _resolution.choices.RemoveAt(0);
            int i = 0;
            foreach (var resolution in Screen.resolutions) {
                _resolution.choices.Add(resolution.width + "x" + resolution.height);
                if (resolution.width == 1920 && resolution.height == 1080) _resolution.index = i;
                i++;
            }
            
            // TODO: set settings to values
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

        private void Back() {
            ChangePanel(_lastPanelBeforeSettings);
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
            GameManager.Instance.LoadScene(GameManager.Scenes.MainMenu);
            ChangePanel(UIs.MainMenu);
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
        private Label _pickupText;

        private void SetupInGame() {
            _interact = _panels[(int) UIs.InGame].Q<VisualElement>("interactIndicator");
            _map = _panels[(int)UIs.InGame].Q<VisualElement>("Map");
            _pickupText = _panels[(int)UIs.InGame].Q<Label>("PickupText");
        }
        
        public void ToggleInteractButton() {
            _interact.style.visibility = _interact.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void SetMap(RenderTexture image) {
            _map.style.backgroundImage = new StyleBackground(RenderTextureTo2DTexture(image));
        }

        public IEnumerator SetPickupText(string text) {
            _pickupText.visible = true;
            _pickupText.text = text;
            yield return new WaitForSeconds(2f);
            _pickupText.visible = false;
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
        private VisualElement _characterPicture;

        public UIController(UIs startPanel) {
            this.startPanel = startPanel;
        }

        private void SetupEncounter() {
            _patience = _panels[(int) UIs.Encounter].Q<LifeMeter>("Patience");
            _enemyName = _panels[(int) UIs.Encounter].Q<Label>("EnemyName");
            _enemyInfo = _panels[(int) UIs.Encounter].Q<Label>("EnemyInfo");
            _cardhand = _panels[(int) UIs.Encounter].Q<Cardhand>("Cardhand");
            _speechLabel = _panels[(int) UIs.Encounter].Q<Label>("SpeechLabel");
            _characterPicture = _panels[(int)UIs.Encounter].Q<VisualElement>("Character");
        }
        
        public void AddCardToHand(Card newCard) {
            _cardhand.AddCard(newCard, _root);
        }

        public void ChangePatience(int patience) {
            _patience.ActiveMeterFields -= patience;
            if (_patience.ActiveMeterFields <= 0) GameManager.Instance.FleeEnemy();
        }

        public int GetPatience() {
            return _patience.ActiveMeterFields;
        }
        
        public void SetEnemyInfo(string enemyName, string enemyInfo) {
            _enemyName.text = enemyName;
            _enemyInfo.text = enemyInfo;
        }

        public void SetSpeech(string text) {
            _speechLabel.text = text;
        }

        public void SetEnemyPicture(Texture2D picture) {
            _characterPicture.style.backgroundImage = new StyleBackground(picture);
        }
        
        #endregion

        #region Key Pad

        private Label _code;
        
        private void SetupKeyPad() {
            _code = _panels[(int) UIs.KeyPad].Q<Label>("Code");
            _panels[(int) UIs.KeyPad].Q<Button>("One").clicked += NumberOne;
            _panels[(int) UIs.KeyPad].Q<Button>("Two").clicked += NumberTwo;
            _panels[(int) UIs.KeyPad].Q<Button>("Three").clicked += NumberThree;
            _panels[(int) UIs.KeyPad].Q<Button>("Four").clicked += NumberFour;
            _panels[(int) UIs.KeyPad].Q<Button>("Five").clicked += NumberFive;
            _panels[(int) UIs.KeyPad].Q<Button>("Six").clicked += NumberSix;
            _panels[(int) UIs.KeyPad].Q<Button>("Seven").clicked += NumberSeven;
            _panels[(int) UIs.KeyPad].Q<Button>("Eight").clicked += NumberEight;
            _panels[(int) UIs.KeyPad].Q<Button>("Nine").clicked += NumberNine;
            _panels[(int) UIs.KeyPad].Q<Button>("Zero").clicked += NumberZero;
            _panels[(int) UIs.KeyPad].Q<Button>("Reset").clicked += Reset;
            _panels[(int) UIs.KeyPad].Q<Button>("Enter").clicked += Enter;
            _panels[(int) UIs.KeyPad].Q<Button>("Close").clicked += CloseKeyPad;
        }

        private void NumberOne() => _code.text += CheckNumberLength() ? "1" : "";
        private void NumberTwo() => _code.text += CheckNumberLength() ? "2" : "";
        private void NumberThree() => _code.text += CheckNumberLength() ? "3" : "";
        private void NumberFour() => _code.text += CheckNumberLength() ? "4" : "";
        private void NumberFive() => _code.text += CheckNumberLength() ? "5" : "";
        private void NumberSix() => _code.text += CheckNumberLength() ? "6" : "";
        private void NumberSeven() => _code.text += CheckNumberLength() ? "7" : "";
        private void NumberEight() => _code.text += CheckNumberLength() ? "8" : "";
        private void NumberNine() => _code.text += CheckNumberLength() ? "9" : "";
        private void NumberZero() => _code.text += CheckNumberLength() ? "0" : "";
        private void Reset() => _code.text = "";

        private void Enter() {
            if (Convert.ToInt32(_code.text) == 24579) {
                PublicEvents.PuzzleIsRight?.Invoke();
                ChangePanel(UIs.InGame);
            }
        }
        private bool CheckNumberLength() => _code.text.Length < 5;
        private void CloseKeyPad() {
            ChangePanel(UIs.InGame);
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
            GameManager.Instance.LoadScene(GameManager.Scenes.Lvl1);
            ChangePanel(UIs.InGame);
        }
        
        // Exit function identical to pause menu

        #endregion
    }
}