using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainSmartphone : MonoBehaviour, IInteractable
{
    public void Interact() {

        Flags.Instance.SetFlags("hasSmartphone", true);

        gameObject.SetActive(false);
    }
}
