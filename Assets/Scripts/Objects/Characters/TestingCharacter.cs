using System;
using System.Collections;
using Manager;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TestingCharacter : MonoBehaviour, IInteractable {
    private void Awake() {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public void Interact() {
        Debug.Log("Interaction");
    }
}