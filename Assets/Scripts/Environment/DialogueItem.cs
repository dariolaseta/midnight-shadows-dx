using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueItem : MonoBehaviour, IInteractable
{
    [TextArea(10, 20)]
    [SerializeField] string dialogue;

    public void Interact() {
        
        DialogueSystem.Instance.SetDialogue(dialogue);
        StartCoroutine(DialogueSystem.Instance.ShowDialogue());
    }
}
