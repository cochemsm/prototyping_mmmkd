using Cards;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class DragAndDrop : MouseManipulator {

        private VisualElement root;
        private VisualElement dragArea;
        private Cardhand cardhand;
        private Vector2 startPosition;

        public DragAndDrop(VisualElement root) {
            this.root = root;
            dragArea = root.Q<VisualElement>("dragArea");
            cardhand = root.Q<Cardhand>("Cardhand");
        }
    
        private void OnMouseDown(MouseDownEvent evt) {
            startPosition = evt.localMousePosition;
        
            dragArea.style.display = DisplayStyle.Flex;
            dragArea.Add(target);
        
            target.CaptureMouse();
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt) {
            if (!target.HasMouseCapture()) return;

            Vector2 move = evt.localMousePosition - startPosition;
            target.style.top = target.layout.y + move.y;
            target.style.left = target.layout.x + move.x;
        
            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt) {
            if (!target.HasMouseCapture()) return;

            if (target.Overlaps(cardhand.contentRect)) {
                cardhand.Q<VisualElement>(className:"cardhandCenter").Add(target);
                Debug.Log("Card Stayed");
            } else {
                target.parent.Remove(target);
                Debug.Log("Removed Card" + target);
            }
            
            dragArea.style.display = DisplayStyle.None;
            cardhand.SetCardsToPoints();
            
            target.ReleaseMouse();
            evt.StopPropagation();
        }
    
        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }
        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }
    }
}