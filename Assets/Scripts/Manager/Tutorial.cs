using System;
using System.Collections;
using UnityEngine;

namespace Manager {
    public class Tutorial : MonoBehaviour {
        private void Start() {
            StartCoroutine(StartTutorial());
        }


        private IEnumerator StartTutorial() {
            yield return new WaitForSeconds(1);
            StartCoroutine(UIController.Instance.SetPickupText("Use 'W/A/S/D' to move"));
            yield return new WaitForSeconds(3);
            StartCoroutine(UIController.Instance.SetPickupText("Use 'Space' to jump"));
        }
    }
}