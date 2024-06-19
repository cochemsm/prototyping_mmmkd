using Interfaces;
using Manager;
using UnityEngine;

namespace Objects.Interactables {
    public class KeyPad : MonoBehaviour, IInteractable {
        public void Interact() {
            UIController.Instance.ChangePanel(UIController.UIs.KeyPad);
            PublicEvents.LockPlayerMovementToggle?.Invoke();
        }
    }
}