using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float lookSpeed = 2f;
    [SerializeField] float lookXLimit = 45f;
    [SerializeField] float defaultHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchSpeed = 3f;

    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lookAction;
    [SerializeField] InputActionReference runAction;
    [SerializeField] InputActionReference crouchAction;

    private float gravity = 10f;
    private float rotationX = 0;
    private float startWalkingSpeed;
    private float startRunningSpeed;

    private bool isMoving = false;
    private bool isPlaying = false;
    private bool isRunning = false;
    private bool isCrouching = false;

    private AudioSource audioSource;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 lookInput;

    private CharacterController characterController;
    private Animator anim;
    private Camera playerCamera;

    private void Awake() {

        ObtainComponent();
    }

    void Start() {

        Init();

        BindInputActions();
    }

    private void OnEnable() {
        
        EnableInputActions();
    }

    private void OnDisable() {

        DisableInputActions();
    }

    private void BindInputActions() {

        moveAction.action.performed += ctx => moveDirection = ctx.ReadValue<Vector3>();
        moveAction.action.canceled += ctx => moveDirection = Vector3.zero;

        lookAction.action.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.action.canceled += ctx => lookInput = Vector2.zero;

        runAction.action.performed += ctx => isRunning = true;
        runAction.action.canceled += ctx => isRunning = false;

        crouchAction.action.performed += ctx => ToggleCrouch();
    }

    private void EnableInputActions() {
        moveAction.action.Enable();
        lookAction.action.Enable();
        runAction.action.Enable();
        crouchAction.action.Enable();
    }

    private void DisableInputActions() {
        moveAction.action.Disable();
        lookAction.action.Disable();
        runAction.action.Disable();
        crouchAction.action.Disable();

        moveAction.action.started -= ctx => moveDirection = Vector3.zero;
        lookAction.action.started -= ctx => lookInput = Vector2.zero;
        runAction.action.started -= ctx => isRunning = false;
        crouchAction.action.started -= ctx => isCrouching = false;
    }

    private void ToggleCrouch() {

        isCrouching = !isCrouching;
        characterController.height = isCrouching ? crouchHeight : defaultHeight;
        walkSpeed = isCrouching ? crouchSpeed : startWalkingSpeed;
        runSpeed = isCrouching ? crouchSpeed : startRunningSpeed;
    }
    
    private void Update() {
        
        CheckForState(GameController.Instance.State);
    }

    private void MoveCharacter() {

        // Determina la velocità di movimento e la direzione
        Vector3 desiredMoveDirection = transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z));
        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        desiredMoveDirection *= currentSpeed;

        // Gestisce la gravità
        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        } else {
            moveDirection.y = 0;
        }

        // Applica il movimento
        characterController.Move((desiredMoveDirection + Vector3.up * moveDirection.y) * Time.deltaTime);

        // Rotazione della telecamera
        rotationX += -lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);

        // Gestione dell'animazione e dell'audio
        isMoving = moveDirection.sqrMagnitude > 0.01f;
        anim.SetFloat("Speed", isMoving ? 1f : 0);
        if (isMoving) {
            if (!isPlaying && audioSource.clip != null) {
                audioSource.Play();
                isPlaying = true;
            }
        } else {
            if (isPlaying && audioSource.clip != null) {
                audioSource.Stop();
                isPlaying = false;
            }
        }

        // Aggiorna il bobbing della telecamera
        if (isRunning)
            CamerabobSystem.Instance.SetBobVelocity(0.008f, 20.0f, 80.0f);
        else
            CamerabobSystem.Instance.ResetBobVelocity();
    }


    private void Init() {

        startWalkingSpeed = walkSpeed;
        startRunningSpeed = runSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ObtainComponent() {

        playerCamera = Camera.main;

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    private void CheckForState(GameState state) {

        switch (state) {
            
            case GameState.FREEROAM:
                MoveCharacter();
                break;
            
            case GameState.OBTAIN_ITEM:
                ObtainItemUI.Instance.CloseItemUI();
                break;
        }
    }
}
