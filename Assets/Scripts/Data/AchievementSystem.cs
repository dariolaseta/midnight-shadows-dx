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

    [Header("Tween settings")] 
    [SerializeField] private float slideInDuration = 0.7f;
    [SerializeField] private float slideOutDuration = 0.5f;
    [SerializeField] private float visibleDuration = 3f;
    [SerializeField] private float slideInOffset = 200f;
    [SerializeField] private Ease slideInEase = Ease.OutBack;
    [SerializeField] private Ease slideOutEase = Ease.InCubic;
    
    private RectTransform achievementRect;
    private Vector2 originalPosition;
    private Vector2 hiddenPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        achievementImg.gameObject.SetActive(false);

        achievementRect = achievementImg.GetComponent<RectTransform>();
        
        bool wasActive = achievementImg.gameObject.activeSelf;
        achievementImg.gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
    
        originalPosition = achievementRect.anchoredPosition;
        hiddenPosition = originalPosition + new Vector2(slideInOffset, 0);
    
        achievementImg.gameObject.SetActive(wasActive);
        achievementRect.anchoredPosition = hiddenPosition;
    }

    public IEnumerator UnlockAchievement(Achievement achievement)
    {
        if (IsAchievementUnlocked(achievement.AchievementID.ToString()))
        {
            yield break;
        }
        
        PlayerPrefs.SetInt(achievement.AchievementID.ToString(), 1);
        PlayerPrefs.Save();
        
        achievementRect.anchoredPosition = hiddenPosition;
        achievementImg.gameObject.SetActive(true);
    
        LayoutRebuilder.ForceRebuildLayoutImmediate(achievementRect);

        achievementRect.DOAnchorPos(originalPosition, slideInDuration)
            .SetEase(slideInEase);
        
        achievementName.text = achievement.AchivementName;
        achievementIcon.sprite = achievement.AchievementIcon;
        
        achievementRect.DOAnchorPos(originalPosition, slideInDuration)
            .SetEase(slideInEase)
            .OnStart(() => achievementImg.gameObject.SetActive(true));
        
        AudioManager.Instance.PlaySfx(achievementSound);

        yield return new WaitForSeconds(visibleDuration);
        
        Sequence exitSequence = DOTween.Sequence();
        exitSequence.Append(achievementRect.DOAnchorPos(hiddenPosition, slideInDuration)
            .SetEase(slideOutEase));

        exitSequence.Join(achievementImg.DOFade(0f, slideOutDuration * 0.5f)
            .SetDelay(slideOutDuration * 0.5f));
        
        exitSequence.OnComplete(() => 
        {
            achievementImg.gameObject.SetActive(false);
            achievementImg.color = new Color(achievementImg.color.r, achievementImg.color.g, achievementImg.color.b, 1f);
        });
        
        yield return exitSequence.WaitForCompletion();
    }

    public bool IsAchievementUnlocked(string achievementId)
    {
        return PlayerPrefs.GetInt(achievementId, 0) == 1;
    }
}
