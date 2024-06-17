using Interfaces;
using Manager;
using UnityEngine;

namespace Objects.Characters {
    public class Character : MonoBehaviour, IInteractable {
        public CharacterData data;
        
        private void Awake() {
            gameObject.tag = "interactable";
        }

        public void Interact() {
            GameManager.Instance.SetCharacter(this);
            UIController.Instance.ChangePanel(UIController.UIs.Encounter);
            UIController.Instance.SetEnemyInfo(data.myName, data.info);
            UIController.Instance.SetEnemyPicture(data.standard);
        }
    }
}