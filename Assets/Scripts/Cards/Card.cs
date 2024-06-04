using UnityEngine;

namespace Cards {
    public abstract class Card : ScriptableObject {
        public string cardName;
        public Texture2D cardImage;
        public Texture2D imageOnCard;
        public string text;
        public int energy;

        public virtual void PlayCard() {
            Debug.Log("Base Card");
        }
    }
}