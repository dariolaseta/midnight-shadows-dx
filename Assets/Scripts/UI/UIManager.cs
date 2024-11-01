using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text instructionLabelObj;
    [SerializeField] private TMP_Text obtainItemLabelObj;
    
    private TMP_Text storyItemText;
    
    private Image storyItemUI;

    private void Awake() {
        
        Init();
    }

    private void Init() {
        
        instructionLabelObj = GameObject.FindGameObjectWithTag("InstructionsLabel").GetComponent<TMP_Text>();
        instructionLabelObj.gameObject.SetActive(false);
        
        obtainItemLabelObj = GameObject.FindGameObjectWithTag("ObtainedText").GetComponent<TMP_Text>();
        obtainItemLabelObj.gameObject.SetActive(false);
        
        storyItemUI = GameObject.FindGameObjectWithTag("StoryItemUI").GetComponent<Image>();
        storyItemText = storyItemUI.GetComponentInChildren<TMP_Text>();

        storyItemUI.gameObject.SetActive(false);

        if (Instance == null) {
            
            Instance = this;
        }
    }

    public void ShowInstructions(bool show, string keyName = "") {
        
        instructionLabelObj.gameObject.SetActive(show);

        if (show) {
            
            instructionLabelObj.text = "Press " + keyName + " to obtain the item";
        }
    }

    public IEnumerator ShowObtainedText(string itemName, GameObject itemObj) {
        
        itemObj.GetComponent<MeshRenderer>().enabled = false;
        itemObj.GetComponent<BoxCollider>().enabled = false;
        
        obtainItemLabelObj.gameObject.SetActive(true);
        obtainItemLabelObj.text = "Obtained " + itemName.ToLower();
        
        yield return new WaitForSeconds(1f);

        yield return obtainItemLabelObj.DOFade(0, 1f).SetEase(Ease.OutCubic).WaitForCompletion();
        
        itemObj.SetActive(false);
    }
    
    public IEnumerator ShowStoryItemObtainText(Items item, GameObject obj, string itemObtainedText = "Obtained ") {

        storyItemUI.gameObject.SetActive(true);
        
        storyItemText.alpha = 1;
        storyItemUI.color = new Color(storyItemUI.color.r, storyItemUI.color.g, storyItemUI.color.b, 1);

        storyItemText.text = itemObtainedText + item.ItemName.ToLower();
        
        yield return new WaitForSeconds(1.5f);

        yield return DOTween.Sequence()
            .Join(storyItemText.DOFade(0f, 1.5f).SetEase(Ease.OutCubic))
            .Join(storyItemUI.DOFade(0f, 1.5f).SetEase(Ease.OutCubic))
            .WaitForCompletion();
        
        storyItemUI.gameObject.SetActive(false);
        
        obj.SetActive(false);
    }
}
