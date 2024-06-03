using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    [SerializeField] private InputActionReference playerinput;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float lookSpeed = 3f;
    private Vector2 input;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    private void OnEnable() {
        playerinput.action.Enable();
    }

    private void Update() {
        input = playerinput.action.ReadValue<Vector2>();
        Vector2 mouseMovement = Mouse.current.delta.ReadValue();
        transform.GetChild(0).rotation = Quaternion.Euler(transform.GetChild(0).rotation.x + mouseMovement.x, transform.GetChild(0).rotation.y + mouseMovement.y, 0);
    }

    private void FixedUpdate() {
        if (input.magnitude > Mathf.Epsilon) rb.velocity = new Vector3(input.x, 0, input.y).normalized * movementSpeed;
        else rb.velocity = Vector3.zero;
    }
}