using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
// using UnityEditor;
using UnityEngine.AI;

// Movement will only work when you bake a nav mesh into the scene,
// make sure ai navigation package is installed.

// Alert system. A number increases the longer the target stays in the fov zone.
// (and decreases when outside of): maxed/being alerted means enemy becomes hostile
public enum AlertStage
{
    Peaceful,
    Aware,
    Alerted
}

public class FieldOfView : MonoBehaviour
{ 
    public AlertStage alertStage;
    [Range(0, 100)] public float alertLevel; // 0=peaceful, 100=alerted

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
    }

    public float movementSpeed = 5;
    public float attackSpeed = 1;
    public float attackCooldown = 2;
    public float damage = 1;
    public Vector3 knockbackForce = new Vector3(0, 2, -5); // intend to scale this on damage
    public float health = 1; // take from enemyhealth later

    [Range(1, 100)] public float aggroSpeed = 20; // how fast alertness increments
    [SerializeField] private bool isAttacking = false;
    //private float maxNumberEnemies = 3; // max number of enemies to be attacking player at once
    // probably need to set this in a game settings later
    [Range(0, 100)] public float radius;
    [Range(0, 360)] public float angle;

    public GameObject playerRef;
    private NavMeshAgent agent;
    [Range(1, 50)] public float enemyDistance = 1.6f;
    //tutorial set this to 0.7f but that seems too small

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    private void Start()
    {
        if (playerRef == null) {
            // playerRef = GameObject.FindGameObjectWithTag("Player");
            // nvm let's just
            playerRef = GameObject.Find("Player");
        }
        StartCoroutine(FOVRoutine());
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
    }

    private IEnumerator FOVRoutine() // core routine to reduce number of calls per frame (for performance)
        // basically: every x s, do this
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Transform target = null;
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        // array of things that collide with your fov cone
        // henceforth we can refer to the rangeChecks array for a list 

        // if there are any colliders in the area
        if (rangeChecks.Length != 0) {
            // Transform target = rangeChecks[0].transform; // this only gets the first in rangecheck
            // Identify player in the array

            foreach (Collider c in rangeChecks) // check list of colliders for player tag
            {
                if (c.CompareTag("Player"))
                    target = c.transform;
                //break; // exit
            }

            if (target == null) // no target to walk towards
            {
                gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                canSeePlayer = false;
            }
            else // target = collider with player tag
            {
                //Vector3 directionToTarget = (target.postion - transform.position).normalized;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                // check for angle
                if ((Vector3.Angle(transform.forward, directionToTarget)) < angle / 2) {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    // limit raycast distance to whenever it hits something (vision is blocked by walls)
                    if (!(Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))) {
                        canSeePlayer = true;
                    }
                    else
                        canSeePlayer = false;
                }
                else
                    canSeePlayer = false; // player is not within radius
            }
        }
        // getting to this point means coliders[] length is zero: auto-fail, so
        else {
            canSeePlayer = false;
        }

        _UpdateAlertState(canSeePlayer);

        if (canSeePlayer) {
            moveTowardsPlayer(target);
        }

    }

    private void _UpdateAlertState(bool playerInFOV) {
        //Debug.Log(alertLevel);
        switch (alertStage) {
            case AlertStage.Peaceful:
                if (playerInFOV)
                    alertStage = AlertStage.Aware;
                break;

            case AlertStage.Aware: // increment if in fov, decrement if not
                if (playerInFOV) {
                    //alertLevel++;
                    alertLevel = alertLevel + aggroSpeed;
                    if (alertLevel >= 100)
                        alertStage = AlertStage.Alerted;
                }
                else
                    //alertLevel--;
                    alertLevel = alertLevel - aggroSpeed;
                    if (alertLevel <= 0)
                        alertStage = AlertStage.Peaceful;
                break;

            case AlertStage.Alerted: // decrement if not in fov
                if (!playerInFOV)
                    alertStage = AlertStage.Aware;
                break;
        }
    }

    private void moveTowardsPlayer(Transform player)
    {
        // move towards player
        //transform.LookAt(player);
        ManualLookAt(player.transform.position);

        // only chase when alerted
        if (alertStage == AlertStage.Alerted) {
            //transform.LookAt(player);
            //agent.speed = movementSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        } else {
            //agent.speed = 0;
            agent.SetDestination(agent.transform.position);
            agent.isStopped = true;
        }

        // if distance between player and self is small: stop moving
        //if (Vector3.Distance(transform.position, player.position) <= enemyDistance) {
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) <= enemyDistance) {

            //Debug.Log("stoppp");
            // this supposedly sets the speed of the navmeshagent to zero but it doesn/t
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // and attack
            // gameObject.GetComponent<Animator>().Play("attack");
            if (alertStage == AlertStage.Alerted) {

                // Debug.Log("deal damage:");
                // Debug.Log(damage);

                if (!isAttacking) {
                    //StartCoroutine(AttackSequence(player));
                    //nevermind forgot there was a check for making sure the player is the target
                    StartCoroutine(AttackSequence(playerRef));
                }

            }
        }
    }
    
    private void ManualLookAt(Vector3 player_pos) {
        // Source: ChatGPT
        Vector3 lookDirection = player_pos - transform.position;
        lookDirection.y = 0; // Optionally, ignore the Y axis to keep the enemy level
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
    
    // private void idle()

    // calculate knockback based off of damage
    private void CalcuateKnockback() {
        float x = 0;
        float y = damage;
        float z = -(damage*2);
        knockbackForce = new Vector3(x, y, z);
    }

    private IEnumerator AttackSequence(GameObject target) {
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

        // make sure final position aligns perfectly
        transform.position = targetPosition;
        ApplyKnockback(target);

        // wait for cooldown before attacking again
        //WaitForCooldown(attackCooldown);
        Invoke("ReadyToAttack", attackCooldown);
    }

    // this never ran
     IEnumerator WaitForCooldown(float waitTime = 3) { // after (cooldown) s, set to attack
        yield return new WaitForSecondsRealtime(waitTime);
        isAttacking = false;
        Debug.Log("cooldown complete!");
    }

    private void ReadyToAttack() {
        isAttacking = false;
        Debug.Log("ready to attack!");
    }

    private void ApplyKnockback(GameObject target) {
        Debug.Log("applying knockback");
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


} // end of class
