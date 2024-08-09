using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainSmartphone : MonoBehaviour, IInteractable
{
    [SerializeField] Items item;

    public void Interact() {

        Flags.Instance.SetFlags("hasSmartphone", true);

        StartCoroutine(StoryItemTextBehaviour.Instance.ShowStoryItemObtain(item, gameObject));

        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
