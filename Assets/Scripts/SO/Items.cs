using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new Items")]
public class Items : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] string itemDescription;

    [SerializeField] Sprite itemImage;

    [SerializeField] int quantity;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;

    public Sprite ItemImage => itemImage;

    public int Quantity => quantity;

    public enum ItemType {

        Shrimp,
    }
}
