using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour, IAttack
{
    //[Header("Interface stuff")]
    public float attackSpeed { get; set; }
    public float attackCooldown { get; set; } = 2;
    public float damage { get; set; } = 10;
    public float stunTime { get; set; } = 6;
    public Vector3 knockbackForce { get; set; }
    public bool isAttacking { get; set; }
    public float attackRadius { get; set; } = 3;
    public void WaitForCooldown(float waitTime = 0) {
        Invoke("ReadyToAttack", waitTime);
    }
    public void ReadyToAttack() {
        isAttacking = false;
    }

    // public void AgentReenableCoroutine(NavMeshAgent agent) {
    //     if(stunEnemies) {
    //         StartCoroutine(((IAttack)this).ReenableAgentAfterDelay(agent, stunTime)); 
    //     } else {
    //         StartCoroutine(((IAttack)this).ReenableAgentOnGround(agent));
    //     }
    // }

    public void CalculateKnockback() { // basically override
        float x = 0;
        float y = damage*0.3f;
        float z = -(damage*0.5f);
        knockbackForce = new Vector3(x, y, z);
    }

    [Header("see script for rest of stats/variables. i'm too tired to making backing vars rn")]
    public bool stunEnemies = false;
    public float stunTimeS = 5;
    public LayerMask attackableMask;

    void Start() {
        CalculateKnockback(); // call local one, not the interface's one
        stunTime = stunTimeS;
    }

    void Update() {
        //AttackButton();
    }

    // void AttackButton() {
    //     if (Input.GetKeyDown(KeyCode.Q)) {
    //         // get colliders in range of attack
    //         Collider[] rangeChecks = Physics.OverlapSphere(transform.position, attackRadius, attackableMask);
    //         foreach (Collider c in rangeChecks) // check list of colliders for player tag
    //         {
    //             //Debug.Log("damage dealt!");
    //             //Debug.Log(knockbackForce);

    //             // Collider is the component - get game object parent
    //             // deal damage + knockback to that collider
    //             ((IAttack)this).DealDamage(c.gameObject);
    //             ((IAttack)this).ApplyKnockback(c.gameObject);
    //         }
    //     }
    // }

    // this was supposed to be an extra check to make sure target is hit after movement
    // private bool CheckTargetHit() {
    //     bool targetHit = false;

    //     // intended to be called after movement.
    //     // if player is still within attack radius, return true
    //     Collider[] rangeChecks = Physics.OverlapSphere(transform.position, attackRadius, targetMask);
    //     foreach (Collider c in rangeChecks) {
    //         if (c.CompareTag("Player"))
    //             targetHit = true;
    //         break; // exit
    //     }

    //     return targetHit;
    // }

    // wanted to override attacksequence - can't do that, might just make own seq
    public void PlayerAttackSequence() {
        // Do a swing.


    }


}
