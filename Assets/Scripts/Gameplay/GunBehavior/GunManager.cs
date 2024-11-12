using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    [SerializeField] InputActionReference fireAction;
    [SerializeField] InputActionReference reloadAction;

    [SerializeField] Transform shootingPointTransform;

    [SerializeField] float fireRange = 0f;
    [SerializeField] float nextTimeToFire = 0f;
    [SerializeField] float fireRate = 0f;

    [SerializeField] int maxAmmo = 0;
    [SerializeField] int currentAmmo = 0;

    [SerializeField] Weapons weapon;
    
    [SerializeField] GameObject impactObject;
    
    private ParticleSystem muzzleFlash;

    private void Awake() {
        
        Init();
    }

    private void OnDisable() {
        
        fireAction.action.canceled -= Shoot;
        reloadAction.action.canceled -= Reload;
    }

    private void Init() {
        
        fireAction.action.performed += Shoot;
        reloadAction.action.performed += Reload;
        
        maxAmmo = weapon.MaxAmmo;
        currentAmmo = maxAmmo;
        fireRange = weapon.Range;
        muzzleFlash = weapon.MuzzleFlash;
        nextTimeToFire = weapon.NextTimeToFire;
    }

    private void Reload(InputAction.CallbackContext obj) {
        
        currentAmmo = maxAmmo;
    }

    private void Shoot(InputAction.CallbackContext obj) {
        
        Ray r = new Ray(shootingPointTransform.position, shootingPointTransform.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, fireRange) && GameController.Instance.State == GameState.FREEROAM && Time.time >= nextTimeToFire) {
            
            if (currentAmmo <= 0) return;
            
            nextTimeToFire = Time.time + 1f / fireRate;
            
            currentAmmo--;
            Debug.Log("Shoot " + hitInfo.transform.name + " Current ammo: " + currentAmmo);
            
            impactObject.transform.position = hitInfo.point;
            impactObject.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            impactObject.SetActive(true);

            ParticleSystem impactParticles = impactObject.GetComponent<ParticleSystem>();
            
            if (impactParticles != null) {
                impactParticles.Stop();
                impactParticles.Play(); 
            }
        }
        
        Debug.DrawRay(r.origin, r.direction * fireRange, Color.green, 1f);
    }
    
    // TODO implement
    private void CheckForImpactParticle(string objectTag) {

        switch (objectTag) {
            
            case "Ground":
                break;
        }
    }
}
