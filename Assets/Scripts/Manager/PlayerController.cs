using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    [SerializeField] private InputActionReference playerinput;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float lookSpeed = 0.5f;
    private Vector2 input;
    Vector3 moveDirection = Vector3.zero;
    private float rotationX;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() {
        playerinput.action.Enable();
    }

    private void Update() {
        input = playerinput.action.ReadValue<Vector2>();
        if (input == Vector2.zero) rb.velocity = Vector2.zero;
        rb.MovePosition(transform.position + (transform.forward * input.y * movementSpeed) + (transform.right * input.x * movementSpeed));

        Vector2 mouseMovement = Mouse.current.delta.ReadValue() * lookSpeed;
        rotationX += -mouseMovement.y;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseMovement.x, 0);
    }

    private void FixedUpdate() {
        if (input.magnitude > Mathf.Epsilon) rb.velocity = new Vector3(input.x, 0, input.y).normalized * movementSpeed;
        else rb.velocity = Vector3.zero;
    }
}