using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE, OBTAIN_ITEM, SMARTPHONE, INSPECTING, DOCUMENT }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] GameObject inventoryScreen;
    [SerializeField] GameObject pauseScreen;

    [SerializeField] InputActionReference inventoryAction;
    [SerializeField] InputActionReference smartphoneAction;
    [SerializeField] InputActionReference smartphoneLightAction;
    [SerializeField] InputActionReference pauseAction;

    private Light smartphoneLight;

    private Animator playerAnim;
    private Animator smartphoneAnim;
    private Animator[] animators;

    private bool isInventoryOpen = false;
    private bool isSmartphoneOn = false;

    private AudioSource[] audioSources;

    private ParticleSystem[] particleSystems;

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

    // DEBUG
    [SerializeField] TMP_Text versionTxt;

    private const string BuildStatus = "Alpha v.";

    void Awake() {

        CreateInstance();

        ObtainComponents();

        //DEBUG
        GetProjectVersion();
    }

    void Start() {
        
        //DEBUG TODO REMOVE
        Debug.Log($"Current State: {state}");
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {

        EnableInputActions();
    }

    private void OnDisable() {

        DisableInputActions();
    }

    private void EnableInputActions() {

        inventoryAction.action.Enable();
        inventoryAction.action.started += OpenInventory;

        smartphoneAction.action.Enable();
        smartphoneAction.action.started += SmartphoneBeheavior;

        smartphoneLightAction.action.Enable();
        smartphoneLightAction.action.started += SwitchSmartphoneLightOn;

        pauseAction.action.Enable();
        pauseAction.action.started += HandlePauseMenu;
    }

    private void DisableInputActions() {

        inventoryAction.action.Disable();
        inventoryAction.action.started -= OpenInventory;

        smartphoneAction.action.Disable();
        smartphoneAction.action.started -= SmartphoneBeheavior;

        smartphoneLightAction.action.Disable();
        smartphoneLightAction.action.started -= SwitchSmartphoneLightOn;

        pauseAction.action.Disable();
        pauseAction.action.started -= HandlePauseMenu;
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void ObtainComponents() {

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        smartphoneAnim = GameObject.FindGameObjectWithTag("Smartphone").GetComponent<Animator>();
        smartphoneLight = smartphoneAnim.GetComponentInChildren<Light>();
        smartphoneLight.enabled = false;
        smartphoneAnim.gameObject.SetActive(false);
    }

    private void GetProjectVersion() {
        
        versionTxt.text = BuildStatus + " " + Application.version;
    }

    public void ChangeState(GameState newState) {
        
        //DEBUG
        Debug.Log($"Changing State To: {newState}");

        prevState = state;
        state = newState;
    }

    public void GoToPrevState() {
        
        //DEBUG
        Debug.Log($"Returning to Previous State: {prevState}");

        state = prevState;
    }

    private void SmartphoneBeheavior(InputAction.CallbackContext obj) {

        if (!Flags.Instance.IsFlagTrue("hasSmartphone")) return;

        if (state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasSmartphone") && !isSmartphoneOn) {

            smartphoneAnim.gameObject.SetActive(true);

            smartphoneAnim.SetTrigger("ON");

            isSmartphoneOn = true;
        } else if (state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasSmartphone") && isSmartphoneOn) {
            
            smartphoneLight.enabled = false;
            smartphoneAnim.SetTrigger("Close");

            isSmartphoneOn = false;
        }
    }

    private void SwitchSmartphoneLightOn(InputAction.CallbackContext obj) {

        if (isSmartphoneOn && state == GameState.FREEROAM) {

            smartphoneLight.enabled = !smartphoneLight.enabled;
        }
    }

    private void OpenInventory(InputAction.CallbackContext obj) {

        if (!isInventoryOpen && state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasBackpack")) {

            playerAnim.enabled = true;

            ChangeState(GameState.INVENTORY);

            inventoryScreen.SetActive(true);

            EnableCursor();

            isInventoryOpen = true;
            
            Inventory.Instance.ListItems();
        } else if (isInventoryOpen && state == GameState.INVENTORY && Flags.Instance.IsFlagTrue("hasBackpack")) {

            inventoryScreen.SetActive(false);

            playerAnim.enabled = false;

            Inventory.Instance.CleanContentItem();

            DisableCursor();

            GoToPrevState();

            isInventoryOpen = false;
        }
    }

    public void EnableCursor() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandlePauseMenu(InputAction.CallbackContext obj) {


        switch(state) {

            case GameState.FREEROAM:
                Pause();
                break;

            case GameState.PAUSE:
                Resume();
                break;
        }
    }

    public void Pause() {

        SetParticleBehavior(true);

        SetAnimatorSpeed(0);

        SetSoundBehavior(true);

        ChangeState(GameState.PAUSE);

        pauseScreen.SetActive(true);

        EnableCursor();
    }

    public void Resume() {

        GoToPrevState();

        pauseScreen.SetActive(false);

        DisableCursor();

        SetAnimatorSpeed(1);

        SetParticleBehavior(false);

        SetSoundBehavior(false);
    }

    private void SetParticleBehavior(bool isPaused) {

        particleSystems = FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem particle in particleSystems) {

            if (!isPaused)
                particle.Play();
            else
                particle.Pause();
        }
    }

    public void SetSoundBehavior(bool isPaused) {

        audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources) {

            if (!isPaused)
                audioSource.Play();
            else
                audioSource.Pause();
        }
    }

    private void SetAnimatorSpeed(int value) {

        GetAllAnimators();

        foreach (Animator anim in animators) {

            anim.speed = value;
        }
    }

    private void GetAllAnimators() {

        animators = FindObjectsOfType<Animator>();
    }
}
