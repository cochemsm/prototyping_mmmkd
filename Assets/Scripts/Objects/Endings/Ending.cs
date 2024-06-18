using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Objects.Endings {
    [CreateAssetMenu(menuName = "Data/New Ending")]
    public class Ending : ScriptableObject {
         public List<ImageAndText> Sequence;
    }
    
    [Serializable]
    public struct ImageAndText {
        public Texture2D Image;
        public List<string> Text;
    }
}