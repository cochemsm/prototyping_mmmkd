using System;
using Cards;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance {get; private set; }

        public Card testCard;
        
        private void Awake() {
            if (Instance is not null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void OnEnable() {
            PublicEvents.LoadNextLevel += LoadNextLevel;
            PublicEvents.SafeGame += SafeGame;
        }

        private void OnDisable() {
            PublicEvents.LoadNextLevel -= LoadNextLevel;
            PublicEvents.SafeGame -= SafeGame;
        }

        private void OnDestroy() {
            if (Instance == this) Instance = null;
        }

        public void LoadNextLevel() {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (!(SceneManager.sceneCount >= index + 1)) return;
            SceneManager.LoadScene(index + 1);
        }
    
        private void SafeGame() {
            Debug.Log("Insert Code to safe the Game");
        }

        public void PauseToggle() {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            if (UIController.Instance is not null) 
                UIController.Instance.ChangePanel(
                    UIController.Instance.CurrentPanel == UIController.UIs.PauseMenu ? 
                        UIController.UIs.InGame : UIController.UIs.PauseMenu);
        }
    }
}