using UnityEngine;

namespace Objects.Cards {
    [CreateAssetMenu(menuName = "Data/New Card")]
    public class Card : ScriptableObject {
        public string cardName;
        public Texture2D cardBase;
        public Texture2D cardBattery;
        public Texture2D imageOnCard;
        public string text;
        public int energy;
        public int befriendPoints;
        public int killPoints;
    }
}