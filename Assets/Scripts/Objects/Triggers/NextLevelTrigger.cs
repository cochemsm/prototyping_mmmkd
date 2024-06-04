using System;
using System.Collections;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour {
    private PublicEvents events;
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        events.LoadNextLevel += () => { };
        
    }
}