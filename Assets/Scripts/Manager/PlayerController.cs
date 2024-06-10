using System;
using System.Collections;
using Manager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [Header("Setup (Do not touch)")]
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference interactInput;
    [SerializeField] private InputActionReference pauseInput;
    
    [Header("Value to play around")]
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float jumpForce = 1;
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
        Vector3 movementVector = new Vector3(_input.x, Mathf.Epsilon, _input.y);
        movementVector = Quaternion.Euler(0, _cameraRotation, 0) * movementVector;
        _rigidbody.velocity = (movementVector * movementSpeed) + new Vector3(0, _rigidbody.velocity.y, 0);

        /*float currentRotation = _playerBody.eulerAngles.y + 180; TODO: well
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
        if (_input != Vector2.zero) _playerBody.rotation = Quaternion.LookRotation(movementVector);
    }

    private void OnEnable() {
        movementInput.action.Enable();
        jumpInput.action.Enable();
        jumpInput.action.performed += Jump;
        interactInput.action.Enable();
        interactInput.action.performed += Interact;
        pauseInput.action.Enable();
        pauseInput.action.performed += Pause;
    }

    private void OnDisable() {
        movementInput.action.Disable();
        jumpInput.action.Disable();
        jumpInput.action.performed -= Jump;
        interactInput.action.Disable();
        interactInput.action.performed -= Interact;
        pauseInput.action.Disable();
        pauseInput.action.performed -= Pause;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("interactable")) return;
        if (UIController.Instance is not null) UIController.Instance.ToggleInteractButton();
        _character = other.transform.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("interactable")) return;
        if (UIController.Instance is not null) UIController.Instance.ToggleInteractButton();
        _character = null;
    }

    private void Jump(InputAction.CallbackContext ctx) {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hit, 1f)) return;
        if (!hit.transform.CompareTag("Ground")) return;
        _rigidbody.AddForce(new Vector3(0, jumpForce, 0));
    }
    
    private IInteractable _character; // TODO: rework this
    private void Interact(InputAction.CallbackContext ctx) {
        _character?.Interact();
    }

    private void Pause(InputAction.CallbackContext ctx) {
        if (GameManager.Instance is not null) GameManager.Instance.PauseToggle();
    }
}