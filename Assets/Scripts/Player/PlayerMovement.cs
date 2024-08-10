using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private float gravity = 10f;
    private float rotationX = 0;
    private float startWalkingSpeed;
    private float startRunningSpeed;

    private bool isMoving = false;
    private bool isPlaying = false;
    private bool canMove = true;

    private AudioSource audioSource;
    private Vector3 moveDirection = Vector3.zero;

    private CharacterController characterController;
    private Animator anim;
    private Camera playerCamera;

    private void Awake() {
        
        ObtainComponent();
    }

    void Start() {

        Init();
    }
    
    private void Update() {
        
        CheckForState(GameController.Instance.State);
    }

    private void MoveCharacter() {

        if (!canMove) return;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = isRunning ? runSpeed * verticalInput : walkSpeed * verticalInput;
        float curSpeedY = isRunning ? runSpeed * horizontalInput : walkSpeed * horizontalInput;

        Vector3 desiredMoveDirection = transform.TransformDirection(Vector3.forward) * curSpeedX + transform.TransformDirection(Vector3.right) * curSpeedY;

        if (desiredMoveDirection.sqrMagnitude > 0.01f) {
            moveDirection.x = desiredMoveDirection.x;
            moveDirection.z = desiredMoveDirection.z;
            isMoving = true;
        } else {
            moveDirection.x = 0;
            moveDirection.z = 0;
            isMoving = false;
        }

        // Handle crouching
        if (Input.GetKey(KeyCode.C)) {

            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        } else {

            characterController.height = defaultHeight;
            walkSpeed = startWalkingSpeed;
            runSpeed = startRunningSpeed;
        }

        // Apply gravity
        if (!characterController.isGrounded) {
            
            moveDirection.y -= gravity * Time.deltaTime;
        } else {

            moveDirection.y = 0;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Handle rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // Update animation and audio
        if (isMoving) {
            
            anim.SetFloat("Speed", 1f);
            if (!isPlaying && audioSource.clip != null) {
                audioSource.Play();
                isPlaying = true;
            }
        } else {
            anim.SetFloat("Speed", 0);
            if (isPlaying && audioSource.clip != null) {
                audioSource.Stop();
                isPlaying = false;
            }
        }
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
