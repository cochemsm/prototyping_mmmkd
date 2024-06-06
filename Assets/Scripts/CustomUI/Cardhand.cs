using System;
using System.Collections.Generic;
using Cards;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class Cardhand : VisualElement {
        public new class UxmlFactory : UxmlFactory<Cardhand> {}

        private List<Card> cards;
        private List<VisualElement> uiCards;
        private VisualTreeAsset cardTemplate;
        private VisualElement center;
    
        public Cardhand() {
            cards = new List<Card>();
            uiCards = new List<VisualElement>();
            center = new VisualElement();
            center.AddToClassList("cardhandCenter");
            hierarchy.Add(center);
        }

        public void AddCard(Card newCard, VisualElement root) {
            cards.Add(newCard);
            cardTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Data/UI/cardTemplate.uxml");
            VisualElement card = cardTemplate.Instantiate();
            card.userData = newCard;
            card.AddToClassList("cardHover");
            card.AddManipulator(new DragAndDrop(root));
            uiCards.Add(card);
            center.Add(card);
            
            SetCardsToPoints();
        }

        public void RemoveCard(Card oldCard) {
            cards.Remove(oldCard);
            
            SetCardsToPoints();
        }

        public void SetCardsToPoints() {
            Vector2 position = Vector2.zero;
            Vector2 offset = new Vector2(0, -300);
            
            float o = 1f;
            float d = 300;
            
            for (int i = 0; i < cards.Count; i++) {
                float x = (float) i / (cards.Count - 1);
                float w = (Mathf.PI - 2 * o) * x + o;

                position.x = d * Mathf.Cos(w) * cards.Count / 2;
                position.y = d * Mathf.Sin(w);

                position += offset;

                uiCards[i].style.position = Position.Absolute;
                uiCards[i].style.bottom = position.y;
                uiCards[i].style.left = position.x;
            }
        }
    }
}