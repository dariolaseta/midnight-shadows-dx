using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfBehaviour : MonoBehaviour, IInteractable
{
    private Animator anim;

    private bool isOpen = false;

    [SerializeField] bool isTop = true;

    public void Interact() {

        if (!isOpen) {

            isOpen = true;

            OpenDrawer();
        } else {

            isOpen = false;
            
            CloseDrawer();
        }
    }


    void Awake() {
        
        ObtainComponents();
    }

    private void ObtainComponents() {

        anim = GetComponentInParent<Animator>();
    }

    private void OpenDrawer() {

        if (isTop)
            anim.SetTrigger("OpenTop");
        else
            anim.SetTrigger("OpenBottom");
    }

    private void CloseDrawer() {

        if (isTop)
            anim.SetTrigger("CloseTop");
        else
            anim.SetTrigger("CloseBottom");
    }
}
