using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance { get; private set; }
    
    [SerializeField] private Camera inspectCamera;
    
    public Camera InspectCamera => inspectCamera;

    private void Awake() {
        
        Init();
    }

    private void Init() {

        if (Instance == null) {
            
            Instance = this;
        }
        
        inspectCamera = GameObject.FindGameObjectWithTag("InspectCamera").GetComponent<Camera>();
        inspectCamera.gameObject.SetActive(false);
    }
}
