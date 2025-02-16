using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject extraMenu;
    [SerializeField] private GameObject achievementsMenu;

    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] Toggle pixelationToggle;

    [SerializeField] RenderTexture pixelationTexture;
    [SerializeField] GameObject pixelationObj;

    [SerializeField] List<ResItems> resolutions = new List<ResItems>();

    [SerializeField] TMP_Text resolutionLabel;

    [SerializeField] int firstLevelID = 0;

    private int selectedResolution = 0;

    private Camera mainCamera;

    void Awake() {

        mainCamera = Camera.main;

        pixelationTexture = mainCamera.targetTexture;
    }

    void Start() {

        Init();
    }

    private void Init() {

        fullscreenToggle.isOn = Screen.fullScreen;

        vsyncToggle.isOn = QualitySettings.vSyncCount != 0;

        FindResolutions();
    }

    public void StartNewGame() {

        LoadingScene.Instance.LoadScene(firstLevelID);
    }

    // TODO: Capire se tenere
    public void Continue() {

        Debug.Log("Continue");
    }

    public void OpenSettingsMenu() {

        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitToDesktop() {

        Application.Quit();
    }

    private void FindResolutions() {

        bool foundRes = false;

        for(int i = 0; i < resolutions.Count; i++) {

            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical) {

                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }

        if (!foundRes) {

            ResItems newRes = new ResItems();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);

            selectedResolution = resolutions.Count - 1;
            UpdateResLabel();
        }
    }

    public void GoBack() {

        mainMenu.SetActive(true);

        settingsMenu.SetActive(false);
    }

    public void ResLeft() {

        selectedResolution = Mathf.Clamp(selectedResolution - 1, 0, resolutions.Count - 1);

        UpdateResLabel();
    }

    public void ResRight() {

        selectedResolution = Mathf.Clamp(selectedResolution + 1, 0, resolutions.Count - 1);

        UpdateResLabel();
    }

    private void UpdateResLabel() {

        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics() {

        mainCamera.targetTexture = pixelationToggle.isOn ? pixelationTexture : null;

        pixelationObj.SetActive(pixelationToggle.isOn);

        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }

    public void ShowExtraMenu()
    {
        mainMenu.SetActive(false);
        extraMenu.SetActive(true);
    }

    public void BackToMainMenuFromExtraMenu()
    {
        mainMenu.SetActive(true);
        extraMenu.SetActive(false);
    }

    public void ShowAchievementsMenu()
    {
        extraMenu.SetActive(false);
        achievementsMenu.SetActive(true);
    }
    
    public void BackToExtraMenuFromAchievementsMenu()
    {
        extraMenu.SetActive(true);
        achievementsMenu.SetActive(false);
    }
}

[System.Serializable]
public class ResItems {

    public int horizontal;
    public int vertical;
}
