using System;
using Interfaces;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class CardPickUp : MonoBehaviour, IInteractable {
        private void Awake() {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        public void Interact() {
            Debug.Log("Picked Up a Card");
            gameObject.SetActive(false);
        }
    }
}