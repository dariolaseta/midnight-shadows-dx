using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponData")]
public class Weapons : ScriptableObject
{
    [SerializeField] string weaponName;
    [SerializeField] string weaponDescription;
    
    [SerializeField] GameObject weaponPrefab;
    
    [SerializeField] Sprite icon;
    
    [SerializeField] int damage;
    [SerializeField] int maxAmmo;
    
    [SerializeField] float range;
    [SerializeField] float attackSpeed;
    [SerializeField] float reloadTime;
    [SerializeField] float nextTimeToFire;
    [SerializeField] float fireRate;

    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip reloadSound;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem bulletImpactEffect;

    public string WeaponName => weaponName;
    public string WeaponDescription => weaponDescription;
    
    public GameObject WeaponPrefab => weaponPrefab;
    
    public Sprite Icon => icon;
    
    public int Damage => damage;
    public int MaxAmmo => maxAmmo;
    
    public float Range => range;
    
    public float AttackSpeed => attackSpeed;
    public float ReloadTime => reloadTime;
    public float NextTimeToFire => nextTimeToFire;
    public float FireRate => fireRate;
    
    public AudioClip ShootSound => shootSound;
    public AudioClip ReloadSound => reloadSound;
    
    public ParticleSystem MuzzleFlash => muzzleFlash;
    public ParticleSystem BulletImpactEffect => bulletImpactEffect;
}
