using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemPlayer : HealthSystem {
    
    [SerializeField] [Range(0,1)] float dangerPercent = 0.2f;
    // private Health healthComponent;
    // public HealthBar healthBar;

    // void Start() {
    //     // get the Health component from the player GameObject
    //     healthComponent = GetComponent<Health>();
    //     healthBar.SetMaxHealth(healthComponent.maxHealth);
    //     healthBar.SetHealth(healthComponent.maxHealth);

    // }

    // void Update() {
    //     HealthBarControl();
    // }
    // void HealthBarControl() {
    //     if (Input.GetKeyDown(KeyCode.LeftArrow)) {
    //         healthComponent.TakeDamage(10);
    //         UpdateHealthBar();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.RightArrow)) {
    //         healthComponent.Heal(10);
    //         UpdateHealthBar();
    //     }
    // }

    public override void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.SetHealth(healthComponent.currentHealth);

            // If health is lower than 20%, add screen shader
            if (healthComponent.currentHealth <= (healthComponent.maxHealth*dangerPercent)) {
                // do something
            }
        }

    }
}

