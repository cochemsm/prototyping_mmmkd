using UnityEngine;

namespace Objects.Triggers {
    [RequireComponent(typeof(BoxCollider))]
    public class Checkpoint : MonoBehaviour {
        private void Awake() {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            PublicEvents.SafeGame?.Invoke();
            Destroy(gameObject);
        }
    }
}