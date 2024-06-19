using System;
using System.Collections;
using System.Collections.Generic;
using Objects.Cards;
using Objects.Characters;
using Objects.Endings;
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
            AddCardsFromPool(3);
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

        public void LoadNextLevel(int playerHealth) {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (!(SceneManager.sceneCountInBuildSettings >= index + 1)) return;
            SceneManager.LoadScene(index + 1);
            StartCoroutine(SetPlayerHealth(playerHealth));
            if ((int) Scenes.GameOver == index + 1) {
                Debug.Log("Ending Started");
                _reachedEnd = true;
                GameEnd();
            }
        }

        public void LoadScene(Scenes scene) {
            if (scene == Scenes.Lvl1) {
                StartCoroutine(SetPlayerHealth(100));
            }
            SceneManager.LoadScene((int) scene);
        }

        private IEnumerator SetPlayerHealth(int playerHealth) {
            yield return new WaitForSeconds(0.1f);
            _player.Health = playerHealth;
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
        
        public void PreventTriggerBug() {
            _player.ManuelTriggerExit();
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
            UIController.Instance.SetEnemyPicture(_friendlyGoal > _evilGoal ? _character.data.happy : _character.data.angry);
        }

        private void BefriendEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            StartCoroutine(UIController.Instance.SetPickupText("You befriended " + _character.data.name));
            _character.gameObject.tag = "Untagged";
            PreventTriggerBug();
        }

        private void KillEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            UIController.Instance.AddCardToHand(_character.data.killReward);
            StartCoroutine(UIController.Instance.SetPickupText("You killed " + _character.data.name + " to survive"));
            _character.gameObject.SetActive(false);
            PreventTriggerBug();
            _kills++;
        }

        public void FleeEnemy() {
            UIController.Instance.ChangePanel(UIController.UIs.InGame);
            StartCoroutine(UIController.Instance.SetPickupText("Enemy has fled to safety"));
            _character.gameObject.SetActive(false);
            PreventTriggerBug();
        }
        
        #endregion

        #region Endings Logic

        private int _kills = 0;
        private bool _reachedEnd;
        private bool _noteEnding;
        [SerializeField] private List<Ending> ends;
        
        public void GameEnd() {
            if (!_reachedEnd) {
                UIController.Instance.ChangePanel(UIController.UIs.GameOver);
                UIController.Instance.SetGameOverText("You died");
                LoadScene(Scenes.GameOver);
                return;
            }

            if (_noteEnding) {
                UIController.Instance.StartEndingSequence(ends[3]);
                return;
            }
            
            switch (_kills) {
                case 0:
                    UIController.Instance.StartEndingSequence(ends[0]);
                    break;
                case 4:
                    UIController.Instance.StartEndingSequence(ends[2]);
                    break;
                default:
                    UIController.Instance.StartEndingSequence(ends[1]);
                    break;
            }
        }

        #endregion
    }
}