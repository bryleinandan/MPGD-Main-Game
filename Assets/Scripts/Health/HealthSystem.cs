using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [HideInInspector] public Health healthComponent;
    public HealthBar healthBar;

    void Start() {
        // get the Health component from the player GameObject
        healthComponent = GetComponentInChildren<Health>();
        healthBar.SetMaxHealth(healthComponent.maxHealth);
        healthBar.SetHealth(healthComponent.maxHealth);

    }

    void Update() {
        //HealthBarControl();
        UpdateHealthBar();
    }
    
    public virtual void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.SetHealth(healthComponent.currentHealth);
        }
    }
}

