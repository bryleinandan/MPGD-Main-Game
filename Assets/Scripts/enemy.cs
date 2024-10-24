using System.Collections;
using System.Collections.Generic;
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

    public float movementSpeed = 3;
    public float attackSpeed = 1;
    public float damage = 0;
    public float health = 1;

    [Range(1, 100)] public float aggroSpeed = 1; // how fast alertness increments
    //private float maxNumberEnemies = 3; // max number of enemies to be attacking player at once
    // probably need to set this in a game settings later
    [Range(0, 100)] public float radius;
    [Range(0, 360)] public float angle;

    public GameObject playerRef;

    private NavMeshAgent agent;
    public float enemyDistance = 0.7f;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
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

        transform.LookAt(player);

        // only chase when alerted
        if (alertStage == AlertStage.Alerted) {
            agent.SetDestination(player.transform.position);
        }

        // if distance between player and self is small: stop moving
        if (Vector3.Distance(transform.position, player.position) <= enemyDistance) {

            // this supposedly sets the speed of the navmeshagent to zero but it doesn/t
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // let's set destination to self and see if this actually stops it moving
            //agent.SetDestination(transform.position);
            // for some reason this locks itself into not chasing the player ever.

            // and attack
            // gameObject.GetComponent<Animator>().Play("attack");
        }
    }
    // private void idle()
}
