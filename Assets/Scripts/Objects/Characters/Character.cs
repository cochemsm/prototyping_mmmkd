using Interfaces;
using UnityEngine;

namespace Objects.Characters {
    public class Character : MonoBehaviour, IInteractable {
        public CharacterData data;
        
        private void Awake() {
            gameObject.tag = "interactable";
        }

        public void Interact() {
            Debug.Log("Interaction");
        }
    }
}