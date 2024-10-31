using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObjectInspection : MonoBehaviour, IInteractable {

    [SerializeField] GameObject cursorsHolder;

    [SerializeField] Transform inspectPosition;
    
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotationSpeed = 1.0f;

    [SerializeField] InputActionReference rotationAction;
    
    [SerializeField] VolumeProfile volumeProfile;
    
    private DepthOfField depthOfField;
    
    private Vector3 originalPosition;
    
    private Quaternion originalRotation;

    private bool isInspecting = false;

    private Transform playerCamera;

    void Start() {

        Init();
    }

    private void Init() {

        cursorsHolder = GameObject.FindGameObjectWithTag("CursorsHolder");
        inspectPosition = GameObject.FindGameObjectWithTag("InspectPosition").transform;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        playerCamera = Camera.main.transform;

        rotationAction.action.performed += OnRotatePerformed;

        if (volumeProfile.TryGet(out depthOfField)) {
            
            depthOfField.active = false;
        }
    }

    private void OnRotatePerformed(InputAction.CallbackContext context) {

        if (isInspecting) {

            Vector2 rotationInput = context.ReadValue<Vector2>();

            transform.Rotate(playerCamera.up, -rotationInput.x * rotationSpeed, Space.World);
            transform.Rotate(playerCamera.right, rotationInput.y * rotationSpeed, Space.World);
        }
    }

    void OnDestroy() {

        rotationAction.action.performed -= OnRotatePerformed;
    }

    public void Interact() {

        isInspecting = !isInspecting;

        cursorsHolder.SetActive(!isInspecting);

        if (isInspecting) {

            StartExamination();
        } else {

            StopExamination();
        }
    }
    private void StartExamination() {

        GameController.Instance.ChangeState(GameState.INSPECTING);

        transform.position = inspectPosition.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        rotationAction.action.Enable();
        
        depthOfField.active = true;
    }

    private void StopExamination() {

        GameController.Instance.GoToPrevState();

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotationAction.action.Disable();
        
        depthOfField.active = false;
    }
}
