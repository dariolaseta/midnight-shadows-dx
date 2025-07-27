using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE, OBTAIN_ITEM, SMARTPHONE, INSPECTING, DOCUMENT, USE_ITEM, LOCK_PUZZLE, NOTE, TEST }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Input actions")]
    [SerializeField] private InputActionReference pauseAction;

    private List<Animator> animators = new List<Animator>();
    private List<AudioSource> audioSources = new List<AudioSource>();
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

    void Awake() 
    {
        CreateInstance();
    }

    void Start() 
    {
        //DEBUG TODO REMOVE
        MyDebug.Instance.UpdateGameStateText(state.ToString());
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() 
    {
        EnableInputActions();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        animators.Clear();
        audioSources.Clear();
        particleSystems.Clear();
    }

    private void OnDisable() 
    {
        DisableInputActions();
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void EnableInputActions() 
    {
        pauseAction.action.Enable();
        pauseAction.action.started += HandlePauseMenu;
    }

    private void DisableInputActions() 
    {
        pauseAction.action.Disable();
        pauseAction.action.started -= HandlePauseMenu;
    }

    private void CreateInstance() 
    {
        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }
    
    public void RegisterAnimator(Animator anim) => animators.Add(anim);
    public void UnregisterAnimator(Animator anim) => animators.Remove(anim);
    
    public void RegisterAudioSource(AudioSource audioSource) => audioSources.Add(audioSource);
    public void UnregisterAudioSource(AudioSource audioSource) => audioSources.Add(audioSource);
    
    public void RegisterParticleSystem(ParticleSystem particle) => particleSystems.Add(particle);
    public void UnregisterParticleSystem(ParticleSystem particle) => particleSystems.Add(particle);
    
    public void ChangeState(GameState newState) 
    {
        prevState = state;
        state = newState;
        
        //DEBUG
        MyDebug.Instance.UpdateGameStateText(state.ToString());
    }

    public void GoToPrevState() 
    {
        state = prevState;
        
        //DEBUG
        MyDebug.Instance.UpdateGameStateText(state.ToString()); //TODO: CHANGE WITH EVENT
    }

    public void EnableCursor() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandlePauseMenu(InputAction.CallbackContext obj) 
    {
        switch(state) {

            case GameState.FREEROAM:
                Pause();
                break;

            case GameState.PAUSE:
                Resume();
                break;
        }
    }

    public void Pause() 
    {
        SetParticleBehavior(true);

        SetAnimatorSpeed(0);

        SetSoundBehavior(true);

        ChangeState(GameState.PAUSE);

        pauseScreen.SetActive(true);

        EnableCursor();
    }

    public void Resume() 
    {
        GoToPrevState();

        pauseScreen.SetActive(false);

        DisableCursor();

        SetAnimatorSpeed(1);

        SetParticleBehavior(false);

        SetSoundBehavior(false);
    }

    private void SetParticleBehavior(bool isPaused) 
    {
        foreach (ParticleSystem particle in particleSystems) 
        {
            if (!isPaused)
                particle.Play();
            else
                particle.Pause();
        }
    }

    public void SetSoundBehavior(bool isPaused) 
    {
        //TODO: Escludere suoni UI
        foreach (AudioSource audioSource in audioSources) {

            if (!isPaused)
                audioSource.Play();
            else
                audioSource.Pause();
        }
    }

    private void SetAnimatorSpeed(int value) 
    {
        foreach (Animator anim in animators) {

            anim.speed = value;
        }
    } 
}
