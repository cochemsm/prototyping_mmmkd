using Interfaces;
using Manager;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class KeyPad : MonoBehaviour, IInteractable {
        private void Awake() {
            GetComponent<SphereCollider>().isTrigger = true; 
        }

        public void Interact() {
            UIController.Instance.ChangePanel(UIController.UIs.KeyPad);
            PublicEvents.LockPlayerMovementToggle?.Invoke();
        }
    }
}