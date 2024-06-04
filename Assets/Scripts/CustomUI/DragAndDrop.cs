using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MouseManipulator {

    private VisualElement dragArea;
    private Vector2 startPosition;

    public DragAndDrop(VisualElement root) {
        dragArea = root.Q<VisualElement>("dragArea");
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