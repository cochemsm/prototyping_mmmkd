using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [Header("Setup (Do not touch)")]
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference interactInput;
    
    [Header("Value to play around")]
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 1;
    
    private Rigidbody _rigidbody;
    private Vector2 _input;
    private Transform _playerBody;
    private float _cameraRotation;
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _playerBody = transform.GetChild(0);
        _cameraRotation = transform.GetChild(1).transform.eulerAngles.y;
    }

    private void Update() {
        _input = movementInput.action.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Vector3 movementVector = new Vector3(_input.x, 0, _input.y);
        movementVector = Quaternion.Euler(0, _cameraRotation, 0) * movementVector;
        _rigidbody.velocity = (movementVector * movementSpeed) + new Vector3(0, _rigidbody.velocity.y, 0);

        _playerBody.rotation = Quaternion.LookRotation(movementVector);
    }

    private void OnEnable() {
        movementInput.action.Enable();
        interactInput.action.Enable();
        interactInput.action.performed += Interact;
    }

    private void OnDisable() {
        movementInput.action.Disable();
        movementInput.action.Disable();
        interactInput.action.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext ctx) {
        Debug.Log("Player tried to interact with something");
    }
}