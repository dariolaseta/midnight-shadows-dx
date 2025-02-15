using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement/Create new Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private string achievementName;
    [SerializeField] private string achievementDescription;
    
    [SerializeField] private Sprite achievementIcon;
    
    public string AchivementName => achievementName;
    public string AchivementDescription => achievementDescription;
    
    public Sprite AchievementIcon => achievementIcon;
}
