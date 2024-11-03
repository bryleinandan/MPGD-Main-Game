using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to manage player health for now
public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth; // set initial player health to max
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            TakeDamage(10);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Heal(10);
        }
    }

    void TakeDamage (int health) {
        if (currentHealth > 0) { // ensure health doesn't go below 0
            currentHealth -= health;
            healthBar.SetHealth(currentHealth);
        }
    }
    void Heal (int health) { // ensure health doesn't go above maxHealth
        if (currentHealth < maxHealth) {
            currentHealth += health;
            healthBar.SetHealth(currentHealth);
        }
    }
}
