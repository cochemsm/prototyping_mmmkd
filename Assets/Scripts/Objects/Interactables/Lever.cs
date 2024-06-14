using Manager;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class Level : MonoBehaviour {
        private static int _leversTurned = 0;
        private SphereCollider _trigger;
        
        private void Awake() {
            _trigger = GetComponent<SphereCollider>();
            _trigger.isTrigger = true;
        }

        public void Interact() {
            // Lever animation trigger
            _leversTurned++;
            if (_leversTurned >= 3) {
                PublicEvents.PuzzleIsRight?.Invoke();
                _trigger.enabled = false;
                GameManager.Instance.PreventTriggerBug();
            }
        }
    }
}