using UnityEngine;

namespace Objects.Actions {
    public class Disappear : MonoBehaviour {
        private void OnEnable() {
            PublicEvents.PuzzleIsRight += MakeMeDisappear;
        }

        private void OnDisable() {
            PublicEvents.PuzzleIsRight -= MakeMeDisappear;
        }

        private void MakeMeDisappear() => gameObject.SetActive(false);
    }
}