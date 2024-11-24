using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public int maxHunger = 100; // Default max hunger
    private int remainingHunger;
    public HungerBar hungerBar; // Reference to the HungerBar UI

    public float hungerDecayRate = 1f; // Time in seconds to decrease hunger

    private void Start()
    {
        remainingHunger = maxHunger;
        hungerBar.SetMaxHunger(maxHunger);
        StartCoroutine(DecreaseHunger());
    }

    private IEnumerator DecreaseHunger()
    {
        while (remainingHunger > 0)
        {
            yield return new WaitForSeconds(hungerDecayRate);
            UpdateHunger(-5); // Reduce hunger by 1 unit
        }
    }

    public void UpdateHunger(int amount)
    {
        remainingHunger = Mathf.Clamp(remainingHunger + amount, 0, maxHunger);
        hungerBar.SetHunger(remainingHunger);

        Debug.Log($"Hunger updated: {remainingHunger}");
    }
}
