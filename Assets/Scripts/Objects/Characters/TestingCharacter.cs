using System.Collections;
using Manager;
using UnityEngine;

public class TestingCharacter : MonoBehaviour, IInteractable {
    public void Interact() {
        Debug.Log("Interaction");
    }
}