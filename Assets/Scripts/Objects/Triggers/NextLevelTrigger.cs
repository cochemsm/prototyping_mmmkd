using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NextLevelTrigger : MonoBehaviour {
    private void Awake() {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        PublicEvents.LoadNextLevel?.Invoke();
    }
}