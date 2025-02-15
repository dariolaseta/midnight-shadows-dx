using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    [SerializeField] TMP_Text versionTxt;

    private const string BuildStatus = "Alpha v.";
    
    [SerializeField] private Achievement achievement;
    
    [SerializeField] private List<Achievement> achievements = new List<Achievement>();
    
    private void Awake()
    {
        GetProjectVersion();
    }

    private void Update()
    {
        //UnlockAllAchievements();
        //DebugAchievements();
    }

    private void GetProjectVersion() 
    {
        versionTxt.text = BuildStatus + " " + Application.version;
    }

    private void DebugAchievements()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(AchievementSystem.Instance.UnlockAchievement(achievement));
        }
    }

    private void UnlockAllAchievements()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (Achievement achievement in achievements)
            {
                PlayerPrefs.SetInt(achievement.AchievementID.ToString(), 1);
                PlayerPrefs.Save();
            }
            
            Debug.Log("Unlocked all achievements");
        }
    }
}
