using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainBackpack : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject playerBackPack;

    [SerializeField] Items item;

    public void Interact() {
        
        Flags.Instance.SetFlags("hasBackpack", true);

        playerBackPack.SetActive(true);

        //TODO: Add tutorial

        StartCoroutine(UIManager.Instance.ShowStoryItemObtainText(item, gameObject));

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
