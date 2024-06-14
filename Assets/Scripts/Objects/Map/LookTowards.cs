using UnityEngine;

namespace Objects.Map {
    public class LookToward : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void FixedUpdate() {
            Vector3 position = transform.position - target.transform.position;
            Quaternion rotation = Quaternion.LookRotation(position);
            transform.rotation = rotation;
        }
    }
}