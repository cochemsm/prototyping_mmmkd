using Interfaces;
using UnityEngine;

namespace Objects.Characters {
    public class TestingCharacter : MonoBehaviour, IInteractable {
        private void Awake() {
            gameObject.tag = "interactable";
        }

        public void Interact() {
            Debug.Log("Interaction");
        }
    }
}