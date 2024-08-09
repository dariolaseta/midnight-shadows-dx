using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryItemTextBehaviour : MonoBehaviour
{
    private GameObject storyItemTextUI;

    private TMP_Text storyItemText;

    private Animator storyItemAnim;

    [SerializeField] string obtainingItem = "Hai ottenuto ";

    public static StoryItemTextBehaviour Instance { get; private set; }

    void Awake() {

        SetInstance();

        ObtainComponents();
    }

    private void ObtainComponents() {

        storyItemTextUI = GameObject.FindGameObjectWithTag("StoryItemUI");
        storyItemText = storyItemTextUI.GetComponentInChildren<TMP_Text>();

        storyItemAnim = storyItemTextUI.GetComponent<Animator>();

        storyItemTextUI.SetActive(false);
    }

    private void SetInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    public IEnumerator ShowStoryItemObtain(Items item, GameObject obj) {

        storyItemTextUI.SetActive(true);

        storyItemText.text = obtainingItem + item.ItemName.ToLower();

        yield return new WaitForSeconds(1.5f);

        storyItemAnim.SetTrigger("Disappear");

        obj.SetActive(false);
    }
}
