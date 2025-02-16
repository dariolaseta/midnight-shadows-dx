using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE, OBTAIN_ITEM, SMARTPHONE, INSPECTING, DOCUMENT, USE_ITEM, TEST }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Input actions")]
    [SerializeField] private InputActionReference smartphoneAction;
    [SerializeField] private InputActionReference smartphoneLightAction;
    [SerializeField] private InputActionReference pauseAction;

    private Light smartphoneLight;

    private Animator playerAnim;
    private Animator smartphoneAnim;
    private Animator[] animators;

    private bool isSmartphoneOn = false;

    private AudioSource[] audioSources;

    private ParticleSystem[] particleSystems;

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

    void Awake() {

        CreateInstance();

        ObtainComponents();
    }

    void Start() {
        
        //DEBUG TODO REMOVE
        MyDebug.Instance.UpdateGameStateText(state.ToString());
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {

        EnableInputActions();
    }

    private void OnDisable() {

        DisableInputActions();
    }

    private void EnableInputActions() {

        smartphoneAction.action.Enable();
        smartphoneAction.action.started += SmartphoneBeheavior;

        smartphoneLightAction.action.Enable();
        smartphoneLightAction.action.started += SwitchSmartphoneLightOn;

        pauseAction.action.Enable();
        pauseAction.action.started += HandlePauseMenu;
    }

    private void DisableInputActions() {

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

    public void ChangeState(GameState newState) {
        
        prevState = state;
        state = newState;
        
        //DEBUG
        MyDebug.Instance.UpdateGameStateText(state.ToString());
    }

    public void GoToPrevState() {
        
        state = prevState;
        
        //DEBUG
        MyDebug.Instance.UpdateGameStateText(state.ToString()); //TODO: CHANGE WITH EVENT
    }

    private void SmartphoneBeheavior(InputAction.CallbackContext obj) 
    {
        // TODO: MOVE TO SMARTPHONE SCRIPT        
        if (!Flags.Instance.IsFlagTrue(FlagEnum.HAS_SMARTPHONE)) return;

        if (state == GameState.FREEROAM && Flags.Instance.IsFlagTrue(FlagEnum.HAS_SMARTPHONE) && !isSmartphoneOn) {

            smartphoneAnim.gameObject.SetActive(true);

            smartphoneAnim.SetTrigger("ON");

            isSmartphoneOn = true;
        } else if (state == GameState.FREEROAM && Flags.Instance.IsFlagTrue(FlagEnum.HAS_SMARTPHONE) && isSmartphoneOn) {
            
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
