using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI {
    public class DragAndDrop : MouseManipulator {

        private VisualElement _root;
        private readonly VisualElement _dragArea;
        private readonly Cardhand _cardhand;
        private Vector2 _startPosition;
        private Vector2 _localStartPos;

        public DragAndDrop(VisualElement root) {
            _root = root;
            _dragArea = root.Q<VisualElement>("dragArea");
            _cardhand = root.Q<Cardhand>("Cardhand");
        }
    
        private void OnMouseDown(MouseDownEvent evt) {
            _startPosition = evt.localMousePosition;
            var globalStartPos = target.worldBound.position;
            _localStartPos = target.layout.position;
            
            _dragArea.style.display = DisplayStyle.Flex;
            _dragArea.Add(target);

            target.style.top = globalStartPos.y;
            target.style.left = globalStartPos.x + 125; // this is half the card width
        
            target.CaptureMouse();
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt) {
            if (!target.HasMouseCapture()) return;

            Vector2 move = evt.localMousePosition - _startPosition;
            target.style.top = target.layout.y + move.y;
            target.style.left = target.layout.x + move.x;
        
            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt) {
            if (!target.HasMouseCapture()) return;

            bool test = target.Overlaps(_cardhand.contentRect);
            Debug.Log(_cardhand.contentRect + "; " + target.contentRect);
            if (test) {
                _cardhand.Q<VisualElement>(className:"cardhandCenter").Add(target);
                Debug.Log("Card Stayed");
            } else {
                target.parent.Remove(target);
                Debug.Log("Removed Card" + target);
            }

            target.style.top = _localStartPos.y;
            target.style.left = _localStartPos.x;
            
            _dragArea.style.display = DisplayStyle.None;
            _cardhand.SetCardsToPoints();
            
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