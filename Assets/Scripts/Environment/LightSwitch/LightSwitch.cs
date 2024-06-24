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
        
        //TODO: Add animation & fix light
        lightOn = !lightOn;
        pointLight.enabled = lightOn;

        Material[] materials = meshRenderer.materials;
        materials[0] = lightOn ? lightOnMaterial : lightOffMaterial;
        meshRenderer.materials = materials;
    }

    void Awake() {

        ObtainComponents();
    }

    void Start() {
        
        Init();
    }

    private void ObtainComponents() {

        anim = GetComponent<Animator>();

        meshRenderer = pointLight.GetComponentInParent<MeshRenderer>();
    }

    private void Init() {

        lightOffMaterial = meshRenderer.materials[0];
    }
}
