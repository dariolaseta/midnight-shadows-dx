using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePuzzle : MonoBehaviour, IInteractable
{
    [Header("Required Item")] 
    [SerializeField] private Items requiredItem;
    
    [Header("Audio")]
    [SerializeField] private AudioClip successSound;
    
    [Header("Text")]
    [TextArea(3, 10)]
    [SerializeField] private string baseDialogue;

    public void Interact()
    {
        if (!Flags.Instance.IsFlagTrue(FlagEnum.HAS_BACKPACK))
        {
            DialogueSystem.Instance.SetDialogue(baseDialogue);
            StartCoroutine(DialogueSystem.Instance.ShowDialogue());
            
            return;
        }
        
        InventoryManager.Instance.StartUseItem(
            requiredItem,
            (success) =>
            {
                if (success) Success();
                else ShowWrongItemMessage();
            }
        );
    }

    private void Success()
    {
        if (successSound)
            AudioManager.Instance.PlaySfx(successSound);
        
        Debug.Log("Opening door...");
    }

    private void ShowWrongItemMessage()
    {
        DialogueSystem.Instance.SetDialogue("Non sembra essere l'oggetto adatto...");
        StartCoroutine(DialogueSystem.Instance.ShowDialogue());
    }
}
