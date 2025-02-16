using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField] private Items item;
    
    [Header("Achievement")]
    [SerializeField] private Achievement achievement;
    
    private MeshRenderer[] renderers;
    private MeshRenderer meshRenderer;
    
    private BoxCollider boxCollider;

    public void Interact() 
    {
        InventoryManager.Instance.AddItem(item);
        
        CheckForEffect(item.Type);
        
        DeactivateItem();
    }

    private void DeactivateItem()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
 
        if (renderers != null && renderers.Length > 0)
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void CheckForEffect(Items.ItemType type)
    {
        switch (type)
        {
            case Items.ItemType.Shrimp:
                StartCoroutine(AchievementSystem.Instance.UnlockAchievement(achievement));
                break;
            case Items.ItemType.Heal:
                Debug.Log("Heal");
                break;
            
            default:
                Debug.Log("Non gestito");
                break;
        }
    }
}
