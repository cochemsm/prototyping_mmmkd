using UnityEngine;

namespace Objects.Triggers {
    [RequireComponent(typeof(BoxCollider))]
    public class Checkpoint : MonoBehaviour {
        private bool canSafe = true;
        
        private void Awake() {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (!canSafe) return;
            if (!other.CompareTag("Player")) return;
            PublicEvents.SafeGame?.Invoke();
            canSafe = false;
        }
    }
}