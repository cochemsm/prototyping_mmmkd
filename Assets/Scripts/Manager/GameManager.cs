using System;
using System.Collections.Generic;
using Objects.Cards;
using Objects.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance {get; private set; }

        public List<Card> cardPool = new List<Card>();
        
        public enum Scenes { MainMenu, Lvl1, Lvl2, Lvl3, GameOver }

        private PlayerController _player;
        
        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start() {
            AddCardsFromPool(5);
        }

        private void OnEnable() {
            PublicEvents.LoadNextLevel += LoadNextLevel;
            PublicEvents.SafeGame += SafeGame;
            PublicEvents.PlayerNotice += PlayerReferenceSingleton;
        }

        private void OnDisable() {
            PublicEvents.LoadNextLevel -= LoadNextLevel;
            PublicEvents.SafeGame -= SafeGame;
            PublicEvents.PlayerNotice -= PlayerReferenceSingleton;
        }

        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }

        private void PlayerReferenceSingleton(PlayerController player) {
            _player = player;
        }

        public void LoadNextLevel() {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (!(SceneManager.sceneCountInBuildSettings >= index + 1)) return;
            SceneManager.LoadScene(index + 1);
            if (SceneManager.sceneCountInBuildSettings == index + 1) UIController.Instance.ChangePanel(UIController.UIs.GameOver);
        }

        public void LoadScene(Scenes scene) {
            SceneManager.LoadScene((int) scene);
        }
    
        private void SafeGame() {
            Debug.Log("Insert Code to safe the Game");
            // TODO: Game Saving
        }

        public void PauseToggle() {
            if (UIController.Instance is null) return;
            if (UIController.Instance.CurrentPanel != UIController.UIs.SettingsMenu)
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            UIController.Instance.ChangePanel(
                UIController.Instance.CurrentPanel == UIController.UIs.PauseMenu ? 
                    UIController.UIs.InGame : UIController.UIs.PauseMenu);
        }

        public void AddCardsFromPool(int amount) {
            for (int i = 0; i < amount; i++) {
                UIController.Instance.AddCardToHand(cardPool[Random.Range(0, cardPool.Count)]);
            }
        }

        public void AddCardToPool(Card card) {
            cardPool.Add(card);
            StartCoroutine(UIController.Instance.SetPickupText("Added new card to deck"));
        }

        public void GameEnd() {
            Debug.Log("Player Died");
        }
        
        #region Patience Logic

        private Character _character;
        private int _friendlyGoal;
        private int _evilGoal;

        public void SetCharacter(Character character) {
            _character = character;
            _friendlyGoal = character.data.befriendGoal;
            _evilGoal = character.data.killGoal;
        }

        public void CardPlayed(int friendly, int evil, int energy) {
            _friendlyGoal -= friendly;
            _evilGoal -= evil;
            if (_friendlyGoal <= 0) BefriendEnemy();
            if (_evilGoal <= 0) KillEnemy();
            UIController.Instance.ChangePatience(energy);
        }

        private void BefriendEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            // TODO: Befriending
        }

        private void KillEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            AddCardToPool(_character.data.killReward);
            _character.gameObject.SetActive(false);
            _player.ManuelTriggerExit();
        }

        public void FleeEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            StartCoroutine(UIController.Instance.SetPickupText("Enemy has fled to safety"));
            _character.gameObject.SetActive(false);
            _player.ManuelTriggerExit();
        }
        
        #endregion
    }
}