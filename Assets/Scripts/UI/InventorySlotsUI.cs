using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    
    private void OnEnable()
    {
        int leftPadding = inventory.CheckInventorySize() > 1 ? 900 : 200;
        
        layoutGroup.padding = new RectOffset( leftPadding, 0, 0, 0);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
}
