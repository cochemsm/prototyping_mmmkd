using System;
using Interfaces;
using Manager;
using Objects.Cards;
using UnityEngine;

namespace Objects.Interactables {
    [RequireComponent(typeof(SphereCollider))]
    public class CardPickUp : MonoBehaviour, IInteractable {
        [SerializeField] private Card data;
        
        private void Awake() {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        public void Interact() {
            GameManager.Instance.AddCardToPool(data);
            gameObject.SetActive(false);
        }
    }
}