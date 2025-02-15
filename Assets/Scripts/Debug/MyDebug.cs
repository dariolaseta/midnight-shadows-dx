using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    public static MyDebug Instance { get; private set; }
    
    [Header("Version")]
    [SerializeField] TMP_Text versionTxt;

    private const string BuildStatus = "Alpha v.";
    
    [Header("Achievemenets")]
    [SerializeField] private Achievement achievement;
    [SerializeField] private List<Achievement> achievements = new List<Achievement>();
    
    [Header("Game States")]
    [SerializeField] private TMP_Text currentGameStateText;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
        
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

    public void UpdateGameStateText(string currentState)
    {
        currentGameStateText.text = $"Current state: {currentState}";
    }
}
