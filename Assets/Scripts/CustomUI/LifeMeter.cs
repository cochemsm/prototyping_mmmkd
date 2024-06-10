using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CustomUI {
    public class LifeMeter : VisualElement {
        
        public new class UxmlFactory : UxmlFactory<LifeMeter, UxmlTraits> {}
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private readonly UxmlIntAttributeDescription _mMeterFields = new UxmlIntAttributeDescription{ name = "MeterFields", defaultValue = 3 };
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                if (ve is LifeMeter element) element.MeterFields = _mMeterFields.GetValueFromBag(bag, cc);
            }
        }

        private const string MeterClass = "meterComponent";
        private const string FieldClass = "meterField";
        
        private int _meterFields;
        public int MeterFields {
            get => _meterFields;
            set {
                _meterFields = value;
                GenerateMeterFields();
            }
        }
    
        public LifeMeter() {
            GenerateMeterFields();
            this.AddToClassList(MeterClass);
        }

        void GenerateMeterFields() {
            while (this.childCount > 0) {
                this.RemoveAt(0);
            }
            
            for (int i = 0; i < _meterFields; i++) {
                VisualElement temp = new VisualElement();
                temp.AddToClassList(FieldClass);
                hierarchy.Add(temp);
            }
        }
    }
}