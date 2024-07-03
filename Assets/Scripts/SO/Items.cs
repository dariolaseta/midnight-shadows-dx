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

    [SerializeField] GameObject model;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;

    public Sprite ItemImage => itemImage;

    public GameObject Model => model;

    public enum ItemType {

        Shrimp,
    }
}
