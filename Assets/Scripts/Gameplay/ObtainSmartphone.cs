using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainSmartphone : MonoBehaviour, IInteractable
{
    [SerializeField] Items item;

    public void Interact() {

        Flags.Instance.SetFlags(FlagEnum.HAS_SMARTPHONE, true);

        StartCoroutine(UIManager.Instance.ShowStoryItemObtainText(item, gameObject));
        
        InventoryManager.Instance.AddItem(item, false);

        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers) {

            if (renderer != null) {

                renderer.enabled = false;
            }
        }

        BoxCollider collider = gameObject.GetComponent<BoxCollider>();

        if (collider != null) {

            collider.enabled = false;
        }
    }
}
