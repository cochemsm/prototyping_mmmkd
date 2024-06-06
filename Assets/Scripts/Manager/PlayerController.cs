using System;
using System.Collections;
using Unity.Mathematics;
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
    // [SerializeField] private float rotationSpeed = 1;
    
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

    // private float rotation;
    private void FixedUpdate() {
        Vector3 movementVector = new Vector3(_input.x, 0, _input.y);
        movementVector = Quaternion.Euler(0, _cameraRotation, 0) * movementVector;
        _rigidbody.velocity = (movementVector * movementSpeed) + new Vector3(0, _rigidbody.velocity.y, 0);

        /*float currentRotation = _playerBody.eulerAngles.y + 180;
        float wantedRotation = Quaternion.LookRotation(movementVector).eulerAngles.y + 180;
        bool rotationDirection = currentRotation < wantedRotation;
        bool checkIfEqual = Math.Abs(currentRotation - wantedRotation) < 1;
        if (!checkIfEqual && movementVector != Vector3.zero) rotation += (rotationDirection ? 1 : -1) * rotationSpeed;
        if (rotation < 1) rotation = 0;
        if (rotation > 360) rotation -= 360;
        rotation -= rotation % rotationSpeed;
        Debug.Log(rotation);
        Vector3 rotate = new Vector3(0, rotation, 0);
        if (movementVector != Vector3.zero) _playerBody.eulerAngles = rotate;*/
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

    private bool trigger;
    private IInteractable character;
    private void Interact(InputAction.CallbackContext ctx) {
        if (!trigger) return;
        character.Interact();
    }

    private void OnTriggerEnter(Collider other) {
        trigger = true;
    }

    private void OnTriggerExit(Collider other) {
        trigger = false;
    }
}