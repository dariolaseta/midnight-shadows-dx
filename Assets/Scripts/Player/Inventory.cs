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

    [SerializeField] int poolSize = 20;

    private List<GameObject> itemPool = new List<GameObject>();
    

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

    private void GeneratePool() {

        for (int i = 0; i < poolSize; i++) {

            GameObject obj = Instantiate(inventoryItem, itemContent);
            obj.SetActive(false);
            itemPool.Add(obj);
        }
    }

    public void ListItems() {

        foreach(var obj in itemPool) {

            obj.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++) {

            GameObject obj;

            if (i < itemPool.Count) {

                obj = itemPool[i];
            } else {

                obj = Instantiate(inventoryItem, itemContent);
                itemPool.Add(obj);
            }

            obj.SetActive(true);

            var itemIcon = obj.transform.Find("ItemSprite").GetComponent<Image>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();

            itemIcon.sprite = inventory[i].ItemImage;
            itemName.text = inventory[i].ItemName;
        }
    }

    public void CleanContentItem() {

        foreach(Transform item in itemContent) {

            item.gameObject.SetActive(false);
        }
    }

    void Awake() {

        GeneratePool();

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
