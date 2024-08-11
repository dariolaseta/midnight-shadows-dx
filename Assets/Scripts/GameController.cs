using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE, OBTAIN_ITEM, SMARTPHONE }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] GameObject inventoryScreen;
    [SerializeField] GameObject pauseScreen;

    private Light smartphoneLight;

    private Animator playerAnim;
    private Animator smartphoneAnim;
    private Animator[] animators;

    private bool isInventoryOpen = false;
    private bool isSmartphoneON = false;

    private AudioSource[] audioSources;

    private ParticleSystem[] particleSystems;

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

    // DEBUG
    [SerializeField] TMP_Text versionTxt;

    private readonly string buildStatus = "Alpha v.";

    void Awake() {

        CreateInstance();

        ObtainComponents();

        //DEBUG
        GetProjectVersion();
    }

    void Start() {
        
        DontDestroyOnLoad(gameObject);
    }

    void Update() {

        StartCoroutine(OpenInventory());

        SmartphoneBeheavior();

        HandlePauseMenu();
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
        
        versionTxt.text = buildStatus + " " + Application.version;
    }

    public void ChangeState(GameState newState) {

        prevState = state;
        state = newState;
    }

    public void GoToPrevState() {

        state = prevState;
    }

    private void SmartphoneBeheavior() {

        if (!Flags.Instance.IsFlagTrue("hasSmartphone")) return;

        if (Input.GetKeyDown(KeyCode.M) && state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasSmartphone") && !isSmartphoneON) {

            smartphoneAnim.gameObject.SetActive(true);

            smartphoneAnim.SetTrigger("ON");

            isSmartphoneON = true;
        } else if (Input.GetKeyDown(KeyCode.M) && state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasSmartphone") && isSmartphoneON) {
            
            smartphoneLight.enabled = false;
            smartphoneAnim.SetTrigger("Close");

            isSmartphoneON = false;
        }

        if (isSmartphoneON && Input.GetKeyDown(KeyCode.F) && state == GameState.FREEROAM) {

            smartphoneLight.enabled = !smartphoneLight.enabled;
        }
    }

    private IEnumerator OpenInventory() {

        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryOpen && state == GameState.FREEROAM && Flags.Instance.IsFlagTrue("hasBackpack")) {

            playerAnim.enabled = true;

            ChangeState(GameState.INVENTORY);

            playerAnim.SetTrigger("OpenBackpack");

            yield return new WaitForSeconds(1.5f);

            inventoryScreen.SetActive(true);

            EnableCursor();

            isInventoryOpen = true;
            
            Inventory.Instance.ListItems();
        } else if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen && state == GameState.INVENTORY && Flags.Instance.IsFlagTrue("hasBackpack")) {

            playerAnim.SetTrigger("CloseBackpack");

            inventoryScreen.SetActive(false);

            yield return new WaitForSeconds(1.5f);

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

    private void HandlePauseMenu() {

        if (state == GameState.FREEROAM && Input.GetKeyDown(KeyCode.Escape)) {

            Pause();
        } else if (state == GameState.PAUSE && Input.GetKeyDown(KeyCode.Escape)) {

            Resume();
        }
    }

    public void Pause() {

        SetParticleBehavior(true);

        SetAnimatorSpeed(0);

        SetSoundBehavior(true);

        state = GameState.PAUSE;

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
