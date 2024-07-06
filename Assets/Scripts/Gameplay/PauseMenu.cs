using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;

    public void ReturnToGame() {

        GameController.Instance.Resume();
    }

    public void ShowSettings() {

        Debug.Log("Settings");
    }

    public void QuitGame() {

        //TODO: Change to load main menu
        Application.Quit();
    }
}
