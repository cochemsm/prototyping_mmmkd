using UnityEngine;

namespace Objects.Cards {
    [CreateAssetMenu(menuName = "Cards/New Card")]
    public class Card : ScriptableObject {
        public string cardName;
        public Texture2D cardImage;
        public Texture2D imageOnCard;
        public string text;
        public int energy;
        public int befriendPoints;
        public int killPoints;

        public void PlayCard() {
            Debug.Log("Card: " + cardName);
        }
    }
}