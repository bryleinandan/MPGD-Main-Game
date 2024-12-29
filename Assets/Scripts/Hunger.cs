using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public int maxHunger = 100; // Default max hunger
    private int remainingHunger;
    public HungerBar hungerBar; // Reference to the HungerBar UI

    public float hungerDecayRate = 1f; // Time in seconds to decrease hunger
    public float healthDecayRate = 1f;

    public GameObject healthObject;
    public Health health;

    private void Start()
    {
        remainingHunger = maxHunger;
        hungerBar.SetMaxHunger(maxHunger);
        StartCoroutine(DecreaseHunger());

        health = healthObject.GetComponentInChildren<Health>();
    }

    private IEnumerator DecreaseHunger()
    {

        while (remainingHunger > 0)
        {
            yield return new WaitForSeconds(hungerDecayRate);
            UpdateHunger(-5); // Reduce hunger by 5 unit
        }
        while (remainingHunger == 0) {
            
            yield return new WaitForSeconds(healthDecayRate);
            health.TakeDamage(5); // Reduce hunger by 5 unit
            Debug.Log($"Health down bc hunger down: {health.currentHealth}");
            
        }
    }

    public void UpdateHunger(int amount)
    {
        remainingHunger = Mathf.Clamp(remainingHunger + amount, 0, maxHunger);
        hungerBar.SetHunger(remainingHunger);

        Debug.Log($"Hunger updated: {remainingHunger}");
    }
}
