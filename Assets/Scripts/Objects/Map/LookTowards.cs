using UnityEngine;

namespace Objects.Map {
    public class LookToward : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void Awake() {
            transform.rotation = Quaternion.LookRotation(target.transform.position * -1);
        }
    }
}