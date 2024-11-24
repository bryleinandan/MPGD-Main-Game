using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack {
    
    public float attackSpeed { get; set; }
    public float attackCooldown { get; set; }
    public float damage { get; set; }
    public Vector3 knockbackForce { get; set; }
    public bool isAttacking { get; set; }
    public float attackRadius { get; set; }
    Transform transform { get; } // get transform of self

    void Start() {
        CalculateKnockback();
    }

    // calculate knockback based off of damage
    public void CalculateKnockback() {
        float x = 0;
        float y = damage*0.25f;
        float z = -(damage*0.50f);
        knockbackForce = new Vector3(x, y, z);
    }

    public virtual IEnumerator AttackSequence(GameObject target) {
        isAttacking = true;

        Transform targetTransform = target.transform;
        // Initial position of the enemy
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < attackSpeed) // Move toward target with predefined speed
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / attackSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // make sure final position aligns with the target's
        transform.position = targetPosition;

        // Check if hit the target!
        //if (CheckTargetHit()) { ApplyKnockback(target); }
        // caused an error where if you were a certain distance, the enemy would just not attack
        
        // I fervently believe that if the attack movement is short enough then it is unstoppable
        ApplyKnockback(target);

        // deal damage: get health system component
        // check if player has Player tag, then get component accordingly
        if (target.CompareTag("Player")) {
            target.GetComponent<PlayerHealthController>().TakeDamage(damage);
        } else {
            //target.GetComponent<PlayerHealthController>().TakeDamage(damage);
        }
        
        WaitForCooldown(attackCooldown);
    }

    public void WaitForCooldown(float waitTime = 3);
    // in monobehaviour class: please define this and use Invoke("ReadyToAttack", attackCooldown); /waitTime lol

    public void ReadyToAttack();
    // public void ReadyToAttack() {
    //     isAttacking = false;
    //     //Debug.Log("ready to attack!");
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

    private void ApplyKnockback(GameObject target) {
        // the code was supposed to only apply this if the target has a rigidbody to apply knockback to,
        // but the if statement never triggers and I figure if player is the only target we have, it will
        // always have rigid body so
        Vector3 knockbackDirection = (target.transform.position - transform.position).normalized + Vector3.up;
        target.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce.magnitude, ForceMode.Impulse);
       
       // if (target.TryGetComponent<Rigidbody>(out Rigidbody targetRigidbody)) {
        //     Debug.Log("rigidbody target found");
        //     Vector3 knockbackDirection = (target.transform.position - transform.position).normalized + Vector3.up;
        //     targetRigidbody.AddForce(knockbackDirection * knockbackForce.magnitude, ForceMode.Impulse);
        // }
        // else {
        //     target.position += knockbackForce;
        // }
    }


    //If your GameObject starts to collide with another GameObject with a Collider
    // void OnCollisionEnter(Collision collision)
    // {
    //     //Output the Collider's GameObject's name
    //     Debug.Log(collision.collider.name);
    // }

    // //If your GameObject keeps colliding with another GameObject with a Collider, do something
    // void OnCollisionStay(Collision collision)
    // {
    //     //Check to see if the Collider's name is "Chest"
    //     if (collision.collider.name == "Chest")
    //     {
    //         //Output the message
    //         Debug.Log("Chest is here!");
    //     }
    // {



}
