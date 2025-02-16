using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHallwayDoor : MonoBehaviour, IInteractable
{
    [Header("Required Item")] 
    [SerializeField] private Items requiredItem;
    
    [Header("Audio")]
    [SerializeField] private AudioClip successSound;

    public void Interact()
    {
        if (!Flags.Instance.IsFlagTrue("hasBackpack"))
        {
            DialogueSystem.Instance.SetDialogue("Non si apre. Dove ho lasciato la chiave?");
            StartCoroutine(DialogueSystem.Instance.ShowDialogue());
            
            return;
        }
        
        InventoryManager.Instance.StartUseItem(
            requiredItem,
            (success) =>
            {
                if (success) OpenDoor();
                else ShowWrongItemMessage();
            }
        );
    }

    private void OpenDoor()
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
