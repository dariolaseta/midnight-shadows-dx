using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObjectInspection : MonoBehaviour, IInteractable
{

    [SerializeField] Items item;

    [SerializeField] GameObject cursorsHolder;

    [SerializeField] Transform inspectPosition;
    
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotationSpeed = 1.0f;

    [SerializeField] InputActionReference rotationAction;
    [SerializeField] InputActionReference inspectInteractAction;
    
    [SerializeField] VolumeProfile volumeProfile;
    
    [SerializeField] bool canObtainItem = false;
    
    private DepthOfField depthOfField;
    
    private Vector3 originalPosition;
    
    private Quaternion originalRotation;

    private bool isInspecting = false;

    private Transform playerCamera;
    
    // TODO: Move to another singleton script
    [SerializeField] TMP_Text obtainedText;

    [SerializeField] TMP_Text instructionLabel;

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
        inspectInteractAction.action.performed += OnItemObtained;
        
        // TODO: Move to other singleton script
        //obtainedText = GameObject.FindGameObjectWithTag("ObtainedText").GetComponent<TextMeshPro>();
        obtainedText.gameObject.SetActive(false);
        
        //instructionLabel = GameObject.FindGameObjectWithTag("InstructionsLabel").GetComponent<TextMeshPro>();
        instructionLabel.gameObject.SetActive(false);

        if (volumeProfile.TryGet(out depthOfField)) {
            
            depthOfField.active = false;
        }
    }

    private void OnItemObtained(InputAction.CallbackContext context)
    {
        if (isInspecting) {
            
            StopExamination(canObtainItem);
        }
    }

    private void OnRotatePerformed(InputAction.CallbackContext context) {

        if (isInspecting) {

            Vector2 rotationInput = context.ReadValue<Vector2>();

            transform.Rotate(playerCamera.up, -rotationInput.x * rotationSpeed, Space.World);
            transform.Rotate(playerCamera.right, rotationInput.y * rotationSpeed, Space.World);
        }
    }

    void OnDisable() {

        rotationAction.action.performed -= OnRotatePerformed;
        inspectInteractAction.action.performed -= OnItemObtained;
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

        if (canObtainItem) {
            
            string keyName = InputControlPath.ToHumanReadableString(inspectInteractAction.action.bindings[0].effectivePath);
            
            // TODO: Cercare un metodo migliore
            if (keyName.Contains("[Keyboard]")) {
                
                keyName = keyName.Replace(" [Keyboard]", "");
            }
        
            instructionLabel.gameObject.SetActive(true);
            instructionLabel.text = "Premi " + keyName + " per ottenere l'oggetto.";
        }

        GameController.Instance.ChangeState(GameState.INSPECTING);

        transform.position = inspectPosition.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        rotationAction.action.Enable();
        
        depthOfField.active = true;
    }

    private void StopExamination(bool itemObtained = false) {

        GameController.Instance.GoToPrevState();

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotationAction.action.Disable();
        
        depthOfField.active = false;
        
        cursorsHolder.SetActive(true);

        if (itemObtained) {
            
            instructionLabel.gameObject.SetActive(false);
            
            Inventory.Instance.AddNewItem(item);
            
            obtainedText.gameObject.SetActive(true);
            obtainedText.text = "Obtained: " + item.name;
            
            gameObject.SetActive(false);
        }
    }
}
