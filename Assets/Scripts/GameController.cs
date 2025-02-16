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
    [SerializeField] private InputActionReference pauseAction;

    private Animator playerAnim;
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

        pauseAction.action.Enable();
        pauseAction.action.started += HandlePauseMenu;
    }

    private void DisableInputActions() {

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
