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
        
        MoveCharacter();
    }

    private void MoveCharacter() {

        if (isMoving) {

            anim.SetFloat("Speed", 1f);
        } else {

            anim.SetFloat("Speed", 0);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = 0;
        float curSpeedY = 0;

        if (canMove) {

            if (isRunning) {
                curSpeedX = runSpeed * Input.GetAxis("Vertical");
                curSpeedY = runSpeed * Input.GetAxis("Horizontal");
            } else {

                curSpeedX = walkSpeed * Input.GetAxis("Vertical");
                curSpeedY = walkSpeed * Input.GetAxis("Horizontal");
            }
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        isMoving = moveDirection.sqrMagnitude > 0.01f;

        if (isMoving && !isPlaying && audioSource.clip != null) {

            audioSource.Play();
            isPlaying = true;
        } else if (!isMoving && isPlaying && audioSource.clip != null) {

            audioSource.Stop();
            isPlaying = false;
        }

        moveDirection.y = movementDirectionY;

        if (Input.GetKey(KeyCode.C) && canMove) {

            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        } else {

            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        if (!characterController.isGrounded) {
            
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove) {

            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void Init() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ObtainComponent() {

        playerCamera = Camera.main;

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }
}
