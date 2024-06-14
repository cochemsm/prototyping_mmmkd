using UnityEngine;

namespace Objects.Map {
    public class LookToward : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void FixedUpdate() {
            if (target == null) {
                Debug.LogWarning("Target for arrow never set");
                return;
            }
            Vector3 position = transform.position - target.transform.position;
            Quaternion rotation = Quaternion.LookRotation(position);
            transform.rotation = rotation;
        }
    }
}