using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement/Create new Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private int achievementID;
    
    [SerializeField] private string achievementName;
    [SerializeField] private string achievementDescription;
    [SerializeField] private string lockedAchievementName;
    [SerializeField] private string lockedAchievementDescription;
    
    [SerializeField] private Sprite achievementIcon;
    
    public string AchivementName => achievementName;
    public string AchivementDescription => achievementDescription;
    public string LockedAchievementName => lockedAchievementName;
    public string LockedAchievementDescription => lockedAchievementDescription;
    
    public int AchievementID => achievementID;
    
    public Sprite AchievementIcon => achievementIcon;
}
