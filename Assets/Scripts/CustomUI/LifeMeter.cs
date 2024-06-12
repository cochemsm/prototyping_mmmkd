using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class LifeMeter : VisualElement {
        
        public new class UxmlFactory : UxmlFactory<LifeMeter, UxmlTraits> {}
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private readonly UxmlIntAttributeDescription _mMeterFields = new UxmlIntAttributeDescription{ name = "MeterFields", defaultValue = 3 };
            private readonly UxmlIntAttributeDescription _mActiveMeterFields = new UxmlIntAttributeDescription{ name = "ActiveFields", defaultValue = 3 };
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                if (ve is not LifeMeter element) return;
                element.MeterFields = _mMeterFields.GetValueFromBag(bag, cc);
                element.ActiveMeterFields = _mActiveMeterFields.GetValueFromBag(bag, cc);
            }
        }

        private const string MeterClass = "meterComponent";
        private const string FieldClass = "meterField";
        private const string ActiveMeterClass = "activeMeterField";
        
        private int _meterFields;
        public int MeterFields {
            get => _meterFields;
            set {
                _meterFields = value;
                if (_meterFields < 0) _meterFields = 0;
                GenerateMeterFields();
            }
        }

        private int _activeMeterFields;
        public int ActiveMeterFields {
            get => _activeMeterFields;
            set {
                _activeMeterFields = Mathf.Clamp(value, 0, MeterFields);;
                GenerateActiveMeterFields();
            }
        }

        private List<VisualElement> fields;
    
        public LifeMeter() {
            GenerateMeterFields();
            AddToClassList(MeterClass);
            fields = new List<VisualElement>();
        }

        private void GenerateMeterFields() {
            while (this.childCount > 0) {
                RemoveAt(0);
            }
            
            for (int i = 0; i < _meterFields; i++) {
                VisualElement temp = new VisualElement();
                temp.AddToClassList(FieldClass);
                hierarchy.Add(temp);
                fields.Add(temp);
            }
        }

        private void GenerateActiveMeterFields() {
            for (int i = 0; i < fields.Count; i++) {
                if (i < ActiveMeterFields) {
                    fields[i].AddToClassList(ActiveMeterClass);
                }
                else {
                    if (fields[i].ClassListContains(ActiveMeterClass)) fields[i].RemoveFromClassList(ActiveMeterClass);
                }
            }
        }
    }
}