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

    [Range(1, 20)] public float aggroSpeed = 1; // how fast alertness increments
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
        
        // do not ask why Length must be capitalised here. blame 2023.1
        if (rangeChecks.Length != 0)
        {
            // Transform target = rangeChecks[0].transform; // this only gets the first in rangecheck
            // Identify player in the array

            foreach (Collider c in rangeChecks) // if there exists an object with player tag in the list of colliders
            {
                if (c.CompareTag("Player"))
                    target = c.transform;
                //break; // exit
            }

            if (target == null) // target is null, so set nav agent velocity to zero as player isn't in range
            {
                gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            }
            else
            {
                //Vector3 directionToTarget = (target.postion - transform.position).normalized;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                // check for angle
                if ((Vector3.Angle(transform.forward, directionToTarget)) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    // limit raycast distance to whenever it hits something (vision is blocked by walls)
                    if (!(Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)))
                    {
                        canSeePlayer = true;;
                    }
                    else
                        canSeePlayer = false;
                }
                else
                    canSeePlayer = false; // player is not within radius
            }
            //else // check failed - for example there are no colliders in the list
            canSeePlayer = false;
            // if check failed and you were previously viewing, you are no longer able to view player

            _UpdateAlertState(canSeePlayer);
            moveTowardsPlayer(target);
        }

    }

    private void _UpdateAlertState(bool playerInFOV)
    {
        //Debug.Log(alertLevel);
        switch (alertStage)
        {
            case AlertStage.Peaceful:
                if (playerInFOV)
                    alertStage = AlertStage.Aware;
                break;

            case AlertStage.Aware: // increment if in fov, decrement if not
                if (playerInFOV)
                {
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

        agent.SetDestination(player.transform.position);

        // if distance between player and self is 0
        if (Vector3.Distance(transform.position, player.position) < enemyDistance)
        {
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            // and attack
        }
    }
    // private void idle()
}
