using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainBackpack : MonoBehaviour, IInteractable
{
    [SerializeField] private Items item;

    public void Interact() {
        
        Flags.Instance.SetFlags(FlagEnum.HAS_BACKPACK, true);

        //TODO: Add tutorial

        StartCoroutine(UIManager.Instance.ShowStoryItemObtainText(item, gameObject));

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
