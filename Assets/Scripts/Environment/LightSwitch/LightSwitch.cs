using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] bool lightOn = false;

    [SerializeField] Light pointLight;

    [SerializeField] Material lightOnMaterial;

    private Material lightOffMaterial;

    private Animator anim;

    private MeshRenderer meshRenderer;

    public void Interact() {
        
        InteractWithLightSwitch();
    }

    void Awake() {

        ObtainComponents();
    }

    void Start() {
        
        Init();
    }

    private void ObtainComponents() {

        anim = GetComponentInChildren<Animator>();

        meshRenderer = pointLight.GetComponentInParent<MeshRenderer>();
    }

    private void Init() {

        lightOffMaterial = meshRenderer.materials[0];
    }

    private void InteractWithLightSwitch() {

        lightOn = !lightOn;
        pointLight.enabled = lightOn;

        anim.SetBool("isOn", lightOn);

        Material[] materials = meshRenderer.materials;
        materials[0] = lightOn ? lightOnMaterial : lightOffMaterial;
        meshRenderer.materials = materials;
    }
}
