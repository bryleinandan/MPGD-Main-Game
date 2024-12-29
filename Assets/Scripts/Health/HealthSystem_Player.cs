using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemPlayer : HealthSystem {
    
    [SerializeField] [Range(0,1)] float dangerPercent = 0.2f;

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

