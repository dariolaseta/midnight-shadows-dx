using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] GameObject inventoryScreen;

    private bool isInventoryOpen = false;

    // DEBUG
    [SerializeField] TMP_Text versionTxt;

    private string buildStatus = "Alpha v.";

    private GameState state = GameState.FREEROAM;
    private GameState prevState;
    public GameState State => state;

    void Awake() {

        CreateInstance();

        //DEBUG
        GetProjectVersion();
    }

    void Start() {
        
        DontDestroyOnLoad(gameObject);
    }

    void Update() {

        OpenInventory();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    private void OpenInventory() {

        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryOpen && state == GameState.FREEROAM) {
            
            inventoryScreen.SetActive(true);

            EnableCursor();

            ChangeState(GameState.INVENTORY);

            isInventoryOpen = true;
            Inventory.Instance.ListItems();
        } else if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen && state == GameState.INVENTORY) {

            Inventory.Instance.CleanContentItem();

            inventoryScreen.SetActive(false);

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
