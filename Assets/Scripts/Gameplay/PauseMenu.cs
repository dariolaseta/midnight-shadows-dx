using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    
    [SerializeField] private int sceneIndexToLoad = 0;

    public void ReturnToGame() 
    {
        GameController.Instance.Resume();
    }

    public void ShowSettings() 
    {
        settingsMenu.SetActive(true);

        gameObject.SetActive(false);
    }

    public void QuitGame() 
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
