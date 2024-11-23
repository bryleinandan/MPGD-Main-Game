using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // initialize health
    }

    public void TakeDamage(int damage) {
        if (currentHealth > 0) {
            currentHealth -= damage;

            if (currentHealth <= 0) { Die(); }
        }
    }

    public void Heal(int amount) {
        if (currentHealth < maxHealth) { currentHealth += amount; }
    }

    protected virtual void Die() {
        // Override this method in subclasses if needed
        Debug.Log($"{gameObject.name} died lol");
    }
}