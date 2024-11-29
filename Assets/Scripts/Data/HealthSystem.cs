using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 0;
    
    void Start() {
        
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        
        currentHealth -= damage;

        if (currentHealth <= 0) {
            
            gameObject.SetActive(false);
        }
    }
}
