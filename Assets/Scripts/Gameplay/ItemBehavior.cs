using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] Items item;

    public void Interact() {

        Inventory.Instance.AddNewItem(item);

        gameObject.SetActive(false);
    }
}
