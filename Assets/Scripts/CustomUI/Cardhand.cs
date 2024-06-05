using System.Collections.Generic;
using Cards;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class Cardhand : VisualElement {
        public new class UxmlFactory : UxmlFactory<Cardhand> {}

        private List<Card> cards;
        private VisualTreeAsset cardTemplate;
    
        public Cardhand() {
            cards = new List<Card>();
        }

        public void AddCard(Card newCard, VisualElement root) {
            Debug.Log("Im here");
            cards.Add(newCard);
            cardTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Data/UI/cardTemplate.uxml");
            VisualElement card = cardTemplate.Instantiate();
            card.userData = newCard;
            card.AddToClassList("card");
            card.AddManipulator(new DragAndDrop(root));
            hierarchy.Add(card);
        }

        public void RemoveCard(Card oldCard) {
            foreach (var card in cards) {
                if (oldCard == card) {
                    cards.Remove(oldCard);
                    return;
                }
            }
        }

        private void SetCardsToPoints() {
        
        }
    }
}