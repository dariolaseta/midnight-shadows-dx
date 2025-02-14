using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] Items item;

    public void Interact() {

        InventoryManager.Instance.AddItem(item);

        gameObject.SetActive(false);
    }
}
