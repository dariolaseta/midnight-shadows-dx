using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHallwayDoor : MonoBehaviour, IInteractable
{
    [Header("References")] 
    [SerializeField] private Items requiredItem;

    public void Interact()
    {
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
        Debug.Log("Opening door...");
    }

    private void ShowWrongItemMessage()
    {
        DialogueSystem.Instance.SetDialogue("Non sembra essere l'oggetto adatto...");
        StartCoroutine(DialogueSystem.Instance.ShowDialogue());
    }
}
