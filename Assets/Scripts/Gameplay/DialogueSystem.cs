using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;

    private bool isShowingDialogue = false;

    public static DialogueSystem Instance { get; private set; } 

    public void SetDialogue(string newDialogue) {

        dialogueText.text = newDialogue;
    }

    public IEnumerator ShowDialogue() {

        if (!isShowingDialogue) {

            isShowingDialogue = true;

            dialogueText.gameObject.SetActive(true);

            yield return new WaitForSeconds(2.5f);

            dialogueText.gameObject.SetActive(false);

            yield return new WaitForSeconds(.5f);

            isShowingDialogue = false;
        }

    }

    void Awake() {
        
        CreateInstance();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }
}
