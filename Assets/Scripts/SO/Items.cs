using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new Items")]
public class Items : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] string itemDescription;

    [SerializeField] int value;

    [SerializeField] Sprite itemImage;

    [SerializeField] GameObject model;

    [SerializeField] ItemType itemType;

    public enum ItemType
    {
        Shrimp,
        Heal,
        Other
    }

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    
    public int Value => value;
    
    public Sprite ItemImage => itemImage;

    public GameObject Model => model;
    
    public ItemType Type => itemType;
}
