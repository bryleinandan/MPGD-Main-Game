using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoSpawner : MonoBehaviour
{
    public float radius = 2000;
    public LayerMask enemyLayer;
    //public Enemy no1;

    private void RangeCheck() {
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (Collider c in enemies) // check list of colliders for player tag
            {
                // check what KIND of enemy it is
                // [not applicable rn]
            }
    }
}
