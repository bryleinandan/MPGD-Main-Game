using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [HideInInspector] public Health healthComponent;
    public HealthBar healthBar;

    void Start() {
        // get the Health component from the player GameObject
        healthComponent = GetComponent<Health>();
        healthBar.SetMaxHealth(healthComponent.maxHealth);
        healthBar.SetHealth(healthComponent.maxHealth);

    }

    void Update() {
        //HealthBarControl();
        UpdateHealthBar();
    }

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

    public virtual void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.SetHealth(healthComponent.currentHealth);
        }
    }
}
