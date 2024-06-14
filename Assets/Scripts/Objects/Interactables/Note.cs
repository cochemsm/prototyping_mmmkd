using Manager;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class Note : MonoBehaviour {
        private static int _notesCollected = 0;
        private SphereCollider _trigger;
        
        private void Awake() {
            _trigger = GetComponent<SphereCollider>();
            _trigger.isTrigger = true;
        }

        public void Interact() {
            gameObject.SetActive(false);
            GameManager.Instance.PreventTriggerBug();
            StartCoroutine(UIController.Instance.SetPickupText("Picked up a note"));
            _notesCollected++;
            if (_notesCollected >= 3) {
                // Notify GameManager about the notes Ending
            }
        }
    }
}