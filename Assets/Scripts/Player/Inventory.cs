using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Items> inventory = new List<Items>();

    [SerializeField] Transform itemContent;

    [SerializeField] GameObject inventoryItem;
    

    public static Inventory Instance { get; private set; }

    public void AddNewItem(Items item) {

        inventory.Add(item);

        ObtainItemUI.Instance.ShowObtainingItemUI(item.ItemImage, item.ItemDescription);
    }

    public void RemoveItem(Items item) {

        if (inventory.Contains(item)) {

            inventory.Remove(item);
        } else {

            Debug.Log("Oggetto non presente nell'inventario");
        }
    }

    public void ListItems() {

        foreach(var item in inventory) {

            GameObject obj = Instantiate(inventoryItem, itemContent);

            var itemIcon = obj.transform.Find("ItemSprite").GetComponent<Image>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();

            itemIcon.sprite = item.ItemImage;
            itemName.text = item.ItemName;
        }
    }

    public void CleanContentItem() {

        foreach(Transform item in itemContent) {

            Destroy(item.gameObject);
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
