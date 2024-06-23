using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] bool lightOn = false;

    [SerializeField] Light pointLight;

    private Animator anim;

    public void Interact() {
        
        //TODO: Add animation & fix light

        if (!lightOn) {

            pointLight.enabled = !lightOn;

            lightOn = true;
        } else {

            pointLight.enabled = !lightOn;

            lightOn = false;
        }
    }

    void Awake() {

        ObtainComponents();
    }

    private void ObtainComponents() {

        anim = GetComponent<Animator>();
    }
}
