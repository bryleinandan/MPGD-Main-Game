using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public HealthSystemPlayer playerHealthBar;

    void Start()
    {
        if(playerHealthBar == null) {
            playerHealthBar = FindFirstObjectByType<HealthSystemPlayer>();
        }
    }

    public void TakeDamage(float dmg) {
        playerHealthBar.GetComponent<Health>().TakeDamage((int)dmg);
    }
}
