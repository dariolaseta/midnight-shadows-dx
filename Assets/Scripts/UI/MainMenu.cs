using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuObj;

    [SerializeField] int firstLevelID = 0;

    public void StartNewGame() {

        // TODO: Cambiare con LoadSceneAsync per schermata di caricamento
        SceneManager.LoadScene(firstLevelID);
    }

    public void Continue() {

        Debug.Log("Continue");
    }

    public void Settings() {

        mainMenuObj.SetActive(false);
        // TODO: Attivare settings menu
    }

    public void QuitToDesktop() {

        Application.Quit();

        Debug.Log("Quitting..."); // TODO: Remove
    }
}
