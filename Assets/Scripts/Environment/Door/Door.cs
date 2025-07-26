using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen = false;
    
    [Header("Audio")]
    [SerializeField] private AudioClip openSfx;
    [SerializeField] private AudioClip closeSfx;

    [Header("Animation triggers")] 
    [SerializeField] private string openAnimationTrigger = "OpenDoor";
    [SerializeField] private string closeAnimationTrigger = "CloseDoor";

    private bool isOpening = false;

    private Animator anim;

    void Awake() 
    {
        ObtainComponents();
    }

    public void Interact() 
    {
        StartCoroutine(DoorBehaviour());
    }

    private IEnumerator DoorBehaviour() 
    {
        if (isOpening) yield break;

        isOpening = true;

        if (!isOpen) 
        {
            isOpen = true;
            
            anim.SetTrigger(openAnimationTrigger);

            if (openSfx != null)
            {
                AudioManager.Instance.PlaySfx(openSfx);
            }
            
            yield return new WaitForSeconds(.7f);

            isOpening = false;
        } else 
        {
            isOpen = false;

            anim.SetTrigger(closeAnimationTrigger);

            if (closeSfx != null)
            {
                AudioManager.Instance.PlaySfx(closeSfx);
            }

            yield return new WaitForSeconds(.7f);

            isOpening = false;
        }
    }

    private void ObtainComponents() 
    {
        anim = GetComponent<Animator>();
    }
}
