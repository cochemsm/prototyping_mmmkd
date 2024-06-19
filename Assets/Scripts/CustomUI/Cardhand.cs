using System.Collections.Generic;
using Manager;
using Objects.Cards;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class Cardhand : VisualElement {
        public new class UxmlFactory : UxmlFactory<Cardhand> {}

        private readonly List<Card> _cards;
        private readonly List<VisualElement> _uiCards;
        private VisualTreeAsset _cardTemplate;
        private readonly VisualElement _center;
    
        public Cardhand() {
            _cards = new List<Card>();
            _uiCards = new List<VisualElement>();
            _center = new VisualElement();
            _center.AddToClassList("cardhandCenter");
            hierarchy.Add(_center);
        }

        public void AddCard(Card newCard, VisualElement root) {
            _cards.Add(newCard);
            _cardTemplate = Resources.Load<VisualTreeAsset>("cardTemplate");
            VisualElement card = _cardTemplate.Instantiate();
            card.Q<VisualElement>("CardBase").style.backgroundImage = new StyleBackground(newCard.cardBase);
            card.Q<LifeMeter>("Energy").style.backgroundImage = new StyleBackground(newCard.cardBatteryBase);
            card.Q<VisualElement>("EnergyBase").style.backgroundImage = new StyleBackground(newCard.cardBattery);
            card.Q<Label>("CardTitle").text = newCard.cardName;
            card.Q<Label>("CardText").text = newCard.text;
            card.Q<LifeMeter>("Energy").ActiveMeterFields = newCard.energy;
            card.userData = newCard;
            card.AddToClassList("card");
            card.AddManipulator(new DragAndDrop(root));
            _uiCards.Add(card);
            _center.Add(card);
            
            SetCardsToPoints();
        }

        public void RemoveCard(VisualElement oldCard) {
            _uiCards.Remove(oldCard);
            _cards.Remove((Card) oldCard.userData);
            
            CheckAllCards();
            
            SetCardsToPoints();
        }

        public void CheckAllCards() {
            int x = 0;
            for (int i = 0; i < _uiCards.Count; i++) {
                VisualElement activate = _uiCards[i].Q<VisualElement>("Active");
                if (_cards[i].energy <= UIController.Instance.GetPatience()) {
                    activate.RemoveFromClassList("inactiveCard");
                } else { 
                    activate.AddToClassList("inactiveCard");
                    x++;
                }
            }
            if (_uiCards.Count == x) PublicEvents.NoCardToPlay?.Invoke();
        }

        public void SetCardsToPoints() {
            Vector2 position = Vector2.zero;
            Vector2 offset = new Vector2(0, -300);
            
            float o = 1f;
            float d = 300;
            
            for (int i = 0; i < _cards.Count; i++) {
                float x = (float) i / (_cards.Count - 1);
                float w = (Mathf.PI - 2 * o) * x + o;

                position.x = d * Mathf.Cos(w) * _cards.Count / 2;
                position.y = d * Mathf.Sin(w);

                position += offset;

                _uiCards[i].style.position = Position.Absolute;
                _uiCards[i].style.bottom = position.y;
                _uiCards[i].style.left = position.x;
                _uiCards[i].style.top = StyleKeyword.Auto;
                _uiCards[i].style.right = StyleKeyword.Auto;
            }
        }
    }
}