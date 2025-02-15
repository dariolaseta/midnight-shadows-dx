using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenuManager : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField] private List<Achievement> achievements = new List<Achievement>();
    
    [Header("UI Elements")]
    [SerializeField] private List<GameObject> achievementObjects = new List<GameObject>();

    private void OnEnable()
    {
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            if (AchievementSystem.Instance.IsAchievementUnlocked(achievement.AchievementID.ToString()))
            {
                Image achievementIcon = achievementObjects[achievement.AchievementID].transform.Find("AchievementIcon").GetComponent<Image>();
                TMP_Text achievementTitle = achievementObjects[achievement.AchievementID].transform.Find("AchievementTitle").GetComponent<TMP_Text>();
                TMP_Text achievementDescription = achievementObjects[achievement.AchievementID].transform.Find("AchievementDescription").GetComponent<TMP_Text>();
                
                achievementIcon.sprite = achievement.AchievementIcon;
                achievementTitle.text = achievement.AchievementName;
                achievementDescription.text = achievement.AchievementDescription;
            }
            else
            {
                TMP_Text achievementTitle = achievementObjects[achievement.AchievementID].transform.Find("AchievementTitle").GetComponent<TMP_Text>();
                TMP_Text achievementDescription = achievementObjects[achievement.AchievementID].transform.Find("AchievementDescription").GetComponent<TMP_Text>();
                
                achievementTitle.text = achievement.LockedAchievementName;
                achievementDescription.text = achievement.LockedAchievementDescription;
            }
        }
    }
}
