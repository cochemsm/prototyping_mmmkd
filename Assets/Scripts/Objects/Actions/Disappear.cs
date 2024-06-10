using UnityEngine;

namespace Objects.Actions {
    public class Disappear : MonoBehaviour {
        private void OnEnable() {
            PublicEvents.CodeIsRight += MakeMeDisappear;
        }

        private void OnDisable() {
            PublicEvents.CodeIsRight -= MakeMeDisappear;
        }

        private void MakeMeDisappear() => gameObject.SetActive(false);
    }
}