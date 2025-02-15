using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private Image achievementImg;
    [SerializeField] private Image achievementIcon;
    [SerializeField] private TMP_Text achievementName;
    [SerializeField] private AudioClip achievementSound;
    
    [SerializeField] private Animator animator; //TODO: Remove

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        achievementImg.gameObject.SetActive(false);
    }

    public IEnumerator UnlockAchievement(Achievement achievement)
    {
        achievementImg.gameObject.SetActive(true);
        
        achievementName.text = achievement.AchivementName;
        achievementIcon.sprite = achievement.AchievementIcon;
        
        AudioManager.Instance.PlaySfx(achievementSound);
        
        animator.SetTrigger("popup");
        
        yield return new WaitForSeconds(3f);
        
        animator.SetTrigger("disappear");
        
        // TODO: SAVE ACHIEVEMENT UNLOCK
    }
}
