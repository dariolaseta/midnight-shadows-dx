using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState { FREEROAM, DIALOG, PAUSE, INVENTORY, CUTSCENE }
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }


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
}
