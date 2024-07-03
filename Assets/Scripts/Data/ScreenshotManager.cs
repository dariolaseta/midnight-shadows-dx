using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

    [SerializeField] string screenshotName;

    void Start() {

        TakeScreenshot(desktopPath + "/" + screenshotName + ".png");
    }

    private void TakeScreenshot(string path) {

        if (mainCamera == null) mainCamera = GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(256, 256, 24);
        mainCamera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        mainCamera.Render();

        RenderTexture.active = rt;

        screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);

        mainCamera.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor) DestroyImmediate(rt);
        else DestroyImmediate(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif
    }
}
