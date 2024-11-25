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
    public void AgentReenableCoroutine(NavMeshAgent agent) {
        if(stunEnemies) {
            StartCoroutine(((IAttack)this).ReenableAgentAfterDelay(agent, stunTime)); 
        } else {
            StartCoroutine(((IAttack)this).ReenableAgentOnGround(agent));
        }
    }
    public void CalculateKnockback() { // basically override
        float x = 0;
        float y = damage*0.4f;
        float z = -(damage*0.6f);
        knockbackForce = new Vector3(x, y, z);
    }

    [Header("see script for rest of stats/variables. i'm too tired to making backing vars rn")]
    public bool stunEnemies = true;
    public float stunTimeS = 5;
    public LayerMask attackableMask;

    void Start() {
        CalculateKnockback(); // call local one, not the interface's one
        stunTime = stunTimeS;
    }

    void Update() {
        AttackButton();
    }

    void AttackButton() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            // get colliders in range of attack
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, attackRadius, attackableMask);
            foreach (Collider c in rangeChecks) // check list of colliders for player tag
            {
                //Debug.Log("damage dealt!");
                //Debug.Log(knockbackForce);

                // Collider is the component - get game object parent
                // deal damage + knockback to that collider
                ((IAttack)this).DealDamage(c.gameObject);
                ((IAttack)this).ApplyKnockback(c.gameObject);
            }

        
        }
    }

    //public override IEnumerator AttackSequence(GameObject target) {
        // isAttacking = true;

        // Transform targetTransform = target.transform;
        // // Initial position of the enemy
        // Vector3 startPosition = transform.position;
        // Vector3 targetPosition = targetTransform.position;
        // float elapsedTime = 0f;

        // while (elapsedTime < attackSpeed) // Move toward target with predefined speed
        // {
        //     elapsedTime += Time.deltaTime;
        //     float t = elapsedTime / attackSpeed;
        //     transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        //     yield return null;
        // }

        // // make sure final position aligns with the target's
        // transform.position = targetPosition;

        // // Check if hit the target!
        // //if (CheckTargetHit()) { ApplyKnockback(target); }
        // // caused an error where if you were a certain distance, the enemy would just not attack
        
        // // I fervently believe that if the attack movement is short enough then it is unstoppable
        // ApplyKnockback(target);

        // // deal damage: get health system component
        // // check if player has Player tag, then get component accordingly
        // if (target.CompareTag("Player")) {
        //     target.GetComponent<PlayerHealthController>().TakeDamage(damage);
        // } else {
        //     //target.GetComponent<PlayerHealthController>().TakeDamage(damage);
        // }
        
        // WaitForCooldown(attackCooldown);
    //}



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
