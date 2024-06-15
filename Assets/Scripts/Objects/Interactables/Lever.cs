using Interfaces;
using Manager;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class Level : MonoBehaviour, IInteractable {
        private static int _leversTurned = 0;
        private SphereCollider _trigger;
        
        private void Awake() {
            _trigger = GetComponent<SphereCollider>();
            _trigger.isTrigger = true;
        }

        public void Interact() {
            // Lever animation trigger
            _trigger.enabled = false;
            GameManager.Instance.PreventTriggerBug();
            _leversTurned++;
            if (_leversTurned >= 3) {
                PublicEvents.PuzzleIsRight?.Invoke();
            }
        }
    }
}