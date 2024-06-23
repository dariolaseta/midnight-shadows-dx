using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    public void Interact() {

        Destroy(gameObject);
    }
}
