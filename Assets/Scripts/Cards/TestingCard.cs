using UnityEngine;

namespace Cards {
    [CreateAssetMenu(menuName = "Cards/Testing Card")]
    public class TestingCard : Card {
        public override void PlayCard() {
            Debug.Log("Testing Card");
        }
    }
}