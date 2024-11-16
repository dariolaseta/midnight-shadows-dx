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
    private ParticleSystem impactParticles;

    private void Awake() {
        
        Init();
    }

    private void OnDisable() {
        
        fireAction.action.performed -= Shoot;
        reloadAction.action.performed -= Reload;
    }

    private void Init() {
        
        impactParticles = impactObject.GetComponent<ParticleSystem>();
        
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
        
        if (!CanShoot()) return;
        
        Ray r = new Ray(shootingPointTransform.position, shootingPointTransform.forward);
        
        currentAmmo--;
        
        nextTimeToFire = Time.time + 1f / fireRate;

        if (Physics.Raycast(r, out RaycastHit hitInfo, fireRange)) {
            
            HandleImpact(hitInfo);
        }
        
        Debug.DrawRay(r.origin, r.direction * fireRange, Color.green, 1f);
    }

    private bool CanShoot() {
        
        return currentAmmo > 0 &&
               GameController.Instance.State == GameState.FREEROAM &&
               Time.time >= nextTimeToFire;
    }

    private void HandleImpact(RaycastHit hitInfo) {
        
        impactObject.transform.position = hitInfo.point;
        impactObject.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        impactObject.SetActive(true);

        impactParticles?.Stop();
        impactParticles?.Play();

        HealthSystem targetHealth = hitInfo.collider.GetComponent<HealthSystem>();
        targetHealth?.TakeDamage(weapon.Damage);

        Debug.Log($"Shoot {hitInfo.transform.name} Current ammo: {currentAmmo}");
    }
    
    // TODO implement
    private void CheckForImpactParticle(string objectTag) {

        switch (objectTag) {
            
            case "Ground":
                break;
        }
    }
}
