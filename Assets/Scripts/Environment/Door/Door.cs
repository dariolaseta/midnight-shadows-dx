using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool isOpen = false;

    private bool isOpening = false;

    private Animator anim;

    void Awake() {
        
        ObtainComponents();
    }

    public void Interact() {

        StartCoroutine(DoorBehaviour());
    }

    private IEnumerator DoorBehaviour() {

        if (isOpening) yield break;

        isOpening = true;

        if (!isOpen) {

            isOpen = true;
            
            anim.SetTrigger("OpenDoor");
            
            yield return new WaitForSeconds(.7f);

            isOpening = false;
        } else {

            isOpen = false;

            anim.SetTrigger("CloseDoor");

            yield return new WaitForSeconds(.7f);

            isOpening = false;
        }
    }

    private void ObtainComponents() {

        anim = GetComponent<Animator>();
    }
}
