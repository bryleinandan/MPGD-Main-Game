using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    //[Header("Interface stuff")]
    public float attackSpeed { get; set; }
    public float attackCooldown { get; set; }
    public float damage { get; set; }
    public Vector3 knockbackForce { get; set; }
    public bool isAttacking { get; set; }
    public float attackRadius { get; set; }
    public void WaitForCooldown(float waitTime = 0) {
        Invoke("ReadyToAttack", 0);
        // no cooldown for player :)
    }
    public void ReadyToAttack() {
        isAttacking = false;
    }

    void Start() {
    }

    void Update() {
        AttackButton();
    }

    void AttackButton() {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // get range of attack
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
