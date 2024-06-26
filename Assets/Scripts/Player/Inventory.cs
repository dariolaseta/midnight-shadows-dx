using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Items> inventory = new List<Items>();

    public static Inventory Instance { get; private set; }

    public void AddNewItem(Items item) {

        inventory.Add(item);

        //TODO: Script per aggiornare UI
    }

    public void RemoveItem(Items item) {

        if (inventory.Contains(item)) {

            inventory.Remove(item);
        } else {

            Debug.Log("Oggetto non presente nell'inventario");
        }
    }

    void Awake() {

        CreateInstance();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
