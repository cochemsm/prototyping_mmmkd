using UnityEngine;

namespace Objects.Map {
    public class LookToward : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void FixedUpdate() {
            transform.rotation = Quaternion.LookRotation(target.transform.position * -1);
        }
    }
}