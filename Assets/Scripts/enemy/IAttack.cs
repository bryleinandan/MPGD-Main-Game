using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IAttack {
    
    public float attackSpeed { get; set; }
    public float attackCooldown { get; set; }
    public float damage { get; set; }
    public Vector3 knockbackForce { get; set; }
    public float stunTime { get; set; }
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

    public IEnumerator AttackSequence(GameObject target) {
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

        DealDamage(target);
        
        WaitForCooldown(attackCooldown);
    }

    public void DealDamage(GameObject target) {
        // deal damage: get health system component
        // check if player has Player tag, then get component accordingly
        if (target.CompareTag("Player")) {
            target.GetComponent<PlayerHealthController>().TakeDamage(damage);
        } else {
            // get health controller - or get from parent - and apply damage
            if (target.TryGetComponent(out HealthController health)) {
                health.TakeDamage(damage);
            } else if (target.transform.parent.TryGetComponent(out HealthController health2)) {
                health2.TakeDamage(damage);
            }
            //target.GetComponent<HealthController>().TakeDamage(damage);
        }
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

    public void ApplyKnockback(GameObject target, Vector3 knockbackDirection = new Vector3()) {
        // the code was supposed to only apply this if the target has a rigidbody to apply knockback to,
        // but the if statement never triggers and I figure if player is the only target we have, it will
        // always have rigid body so
        //Vector3 knockbackDirection = (target.transform.position - transform.position).normalized + Vector3.up;
        
        if (knockbackDirection== new Vector3()) {
            knockbackDirection = (target.transform.position - transform.position).normalized + Vector3.up;
        }

        // Navmesh agent must be disabled before applying force as it will snap object to the ground
        if (target.TryGetComponent<NavMeshAgent>(out NavMeshAgent targetNavAgent)) {
            targetNavAgent.enabled = false;
        }

        // punt the guy
        target.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce.magnitude, ForceMode.Impulse);
       
        // if there was a navmeshagent that we disabled, re-enable it after a time
        // if (targetNavAgent != null) {
        //     //ReenableAgentAfterDelay(targetNavAgent, stunTime);
        //     //ReenableAgentOnGround(targetNavAgent);
        //     //targetNavAgent.enabled = true;
        //     AgentReenableCoroutine(targetNavAgent);
        // }
        // this is handled elsewhere.
    }

    // velocity change mode.
    public void ApplyKnockbackIgnoreMass(GameObject target, Vector3 knockbackDirection = new Vector3()) {

        if (knockbackDirection== new Vector3()) {
            knockbackDirection = (target.transform.position - transform.position).normalized + Vector3.up;
        }

        // Navmesh agent must be disabled before applying force as it will snap object to the ground
        if (target.TryGetComponent<NavMeshAgent>(out NavMeshAgent targetNavAgent)) {
            targetNavAgent.enabled = false;
        }

        target.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce.magnitude, ForceMode.VelocityChange);
       
       // if there was a navmeshagent that we disabled, re-enable it after a time
        // if (targetNavAgent != null) {
        //     AgentReenableCoroutine(targetNavAgent);
        // }
    }

    //public void AgentReenableCoroutine(NavMeshAgent agent);
    // call one or the other - this is dictated in fieldofview and playerattack.cs

    public IEnumerator ReenableAgentAfterDelay(NavMeshAgent agent, float delay = 5) {
        yield return new WaitForSeconds(delay);
        if (agent != null) {  
            agent.enabled = true;
        }
    }
       
    public IEnumerator ReenableAgentOnGround(NavMeshAgent agent, float height = 1.5f, float delay = 1.5f) {
        yield return new WaitForSeconds(delay);

        //Debug.Log("reenable agent on ground...");
        
        bool grounded = CheckIfGrounded(height);
        while (!grounded) {
            grounded = CheckIfGrounded(height);
            //Debug.Log("Grounded: " + grounded);
            yield return null;
        }
        //Debug.Log("Object is now grounded!");
        agent.enabled = true;
    }

    bool CheckIfGrounded(float checkDistance) {
        LayerMask groundLayer = (LayerMask.GetMask("ground"));
        //return Physics.Raycast(transform.position, Vector3.down, checkDistance * 0.5f + 0.3f, groundLayer);
        // i don't remember why we 0.5f + 0.3f but maybe it was important. it doesn't work when the enemy is upside down
        return Physics.Raycast(transform.position, Vector3.down, checkDistance *1.1f, groundLayer);
    }

}
