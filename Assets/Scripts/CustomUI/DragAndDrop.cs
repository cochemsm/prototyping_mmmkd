using Manager;
using Objects.Cards;
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
            target.style.left = globalStartPos.x;
        
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
            
            bool test = RectOverlap(target.worldBound, _cardhand.worldBound);
            if (test) {
                _cardhand.Q<VisualElement>(className:"cardhandCenter").Add(target);
                Debug.Log("Card Stayed");
            } else {
                _cardhand.RemoveCard((Card) target.userData);
                target.RemoveFromHierarchy();
                GameManager.Instance.CardPlayed(((Card) target.userData).befriendPoints, ((Card) target.userData).killPoints, ((Card) target.userData).energy);
                Debug.Log("Removed Card");
            }

            target.style.top = _localStartPos.y;
            target.style.left = _localStartPos.x;
            
            _dragArea.style.display = DisplayStyle.None;
            
            target.ReleaseMouse();
            evt.StopPropagation();
        }

        private bool RectOverlap(Rect firstRect, Rect secondRect) {
            Debug.LogWarning($"{firstRect.x}, {firstRect.y}|{firstRect.width}, {firstRect.height}");
            
            return firstRect.x + firstRect.width >= secondRect.x && firstRect.x <= secondRect.x + secondRect.width &&
                   firstRect.y + 350 >= secondRect.y && firstRect.y <= secondRect.y + secondRect.height;
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