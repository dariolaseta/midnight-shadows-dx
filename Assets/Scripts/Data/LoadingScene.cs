using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;

    [SerializeField] Image loadingBarFill;
    
    public static LoadingScene Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void LoadScene(int sceneIndex)
    {

        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex) {
        
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone) {
            
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            
            loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
}
