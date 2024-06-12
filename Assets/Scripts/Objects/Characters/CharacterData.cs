using Objects.Cards;
using UnityEngine;

namespace Objects.Characters {
    public class CharacterData : ScriptableObject {
        public string myName;
        public string info;
        public int befriendGoal;
        public int killGoal;
        public Card killReward;
    }
}