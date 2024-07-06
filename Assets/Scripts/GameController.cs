using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE, OBTAIN_ITEM }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] GameObject inventoryScreen;

    private Animator playerAnim;

    private bool isInventoryOpen = false;

    // DEBUG
    [SerializeField] TMP_Text versionTxt;

    private string buildStatus = "Alpha v.";

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

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
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void ObtainComponents() {

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
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
}
