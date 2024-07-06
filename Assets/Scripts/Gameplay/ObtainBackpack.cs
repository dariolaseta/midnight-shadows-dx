using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainBackpack : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject playerBackPack;

    public void Interact() {
        
        Flags.Instance.SetFlags("hasBackpack", true);

        playerBackPack.SetActive(true);

        //TODO: Add tutorial

        gameObject.SetActive(false);
    }
}
