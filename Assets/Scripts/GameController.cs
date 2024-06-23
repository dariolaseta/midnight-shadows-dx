using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }


    // DEBUG
    [SerializeField] TMP_Text versionTxt;

    private string buildStatus = "Alpha v.";

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
}
