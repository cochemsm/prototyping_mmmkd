using System;
using Cards;
using UnityEngine;

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

        private void LoadNextLevel() {
            Debug.Log("Insert Code to load next level");
        }
    
        private void SafeGame() {
            Debug.Log("Insert Code to safe the Game");
        }
    }
}