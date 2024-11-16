using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    [SerializeField] InputActionReference fireAction;
    [SerializeField] InputActionReference reloadAction;

    [SerializeField] Transform shootingPointTransform;

    [SerializeField] float fireRange = 0f;
    [SerializeField] float fireRate = 0f;
    
    private float nextTimeToFire = 0;

    [SerializeField] int currentAmmo = 0;
    private int magazineSize;
    private int maxAmmo;
    private int totalAmmo = 30;
    [SerializeField] float reloadTime = 1f;
    
    private AudioClip shootSound;
    private AudioClip reloadSound;
    
    private bool isReloading = false;

    [SerializeField] Weapons weapon;
    
    [SerializeField] GameObject impactObject;
    
    private ParticleSystem muzzleFlash;
    private ParticleSystem impactParticles;

    private TMP_Text ammoCounter;

    private void Awake() {
        
        Init();
    }

    private void OnDisable() {
        
        fireAction.action.performed -= Shoot;
        reloadAction.action.performed -= Reload;
        
        ammoCounter.gameObject.SetActive(false);
        
        isReloading = false;
    }

    private void Init() {
        
        impactParticles = impactObject.GetComponent<ParticleSystem>();
        
        ammoCounter = GameObject.FindGameObjectWithTag("AmmoCounter").GetComponentInChildren<TMP_Text>(true);
        ammoCounter.gameObject.SetActive(true);
        
        fireAction.action.performed += Shoot;
        reloadAction.action.performed += Reload;

        maxAmmo = weapon.MaxAmmo;
        fireRate = weapon.FireRate;
        currentAmmo = weapon.MaxAmmo;
        magazineSize = weapon.MagazineSize;
        fireRange = weapon.Range;
        muzzleFlash = weapon.MuzzleFlash;
        reloadTime = weapon.ReloadTime;
        shootSound = weapon.ShootSound;
        reloadSound = weapon.ReloadSound;
    }

    private void Reload(InputAction.CallbackContext obj) {

        StartCoroutine(ReloadCoroutine());
    }

    private void Shoot(InputAction.CallbackContext obj) {
        
        
        if (!CanShoot()) return;
        
        Ray r = new Ray(shootingPointTransform.position, shootingPointTransform.forward);
        
        currentAmmo--;
        
        UpdateAmmoCounterLabel();
        
        AudioManager.Instance.PlaySfx(shootSound);
        
        nextTimeToFire = Time.time + 1f / fireRate;

        if (Physics.Raycast(r, out RaycastHit hitInfo, fireRange)) {
            
            HandleImpact(hitInfo);
        }
        
        Debug.DrawRay(r.origin, r.direction * fireRange, Color.green, 1f);
    }

    private IEnumerator ReloadCoroutine() {
        
        if (!CanReload()) yield break;
        
        // TODO Replace with animator
        Debug.Log("Start Reloading...");
        
        AudioManager.Instance.PlaySfx(reloadSound);
        
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);
        
        int ammoNeeded = maxAmmo - currentAmmo;
        
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);
        
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;
        
        UpdateAmmoCounterLabel();
        
        isReloading = false;
    }

    private bool CanShoot() {
        
        return currentAmmo > 0 &&
               GameController.Instance.State == GameState.FREEROAM &&
               Time.time >= nextTimeToFire &&
               !isReloading;
    }

    private bool CanReload() {
        
        return totalAmmo > 0 && 
               !isReloading && 
               currentAmmo < maxAmmo &&
               GameController.Instance.State == GameState.FREEROAM;
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

    private void UpdateAmmoCounterLabel() {
        
        ammoCounter.text = $"{currentAmmo} / {totalAmmo}";
    }
    
    // TODO implement
    private void CheckForImpactParticle(string objectTag) {

        switch (objectTag) {
            
            case "Ground":
                break;
        }
    }
}
