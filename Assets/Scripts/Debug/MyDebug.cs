using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    [SerializeField] TMP_Text versionTxt;

    private const string BuildStatus = "Alpha v.";
    
    private void Awake()
    {
        GetProjectVersion();
    }

    private void GetProjectVersion() 
    {
        versionTxt.text = BuildStatus + " " + Application.version;
    }
}
