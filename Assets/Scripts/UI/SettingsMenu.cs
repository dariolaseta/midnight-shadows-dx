using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle vsyncToggle;

    [SerializeField] List<ResItems> resolutions = new List<ResItems>();

    [SerializeField] TMP_Text resolutionLabel;

    private int selectedResolution = 0;

    void Start() {

        Init();
    }

    private void Init() {

        fullscreenToggle.isOn = Screen.fullScreen;

        vsyncToggle.isOn = QualitySettings.vSyncCount != 0;

        FindResolutions();
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

        pauseMenu.SetActive(true);

        gameObject.SetActive(false);
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

        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }
}

[System.Serializable]
public class ResItems {

    public int horizontal;
    public int vertical;
}
