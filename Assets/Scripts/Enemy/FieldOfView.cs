using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
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

public class FieldOfView : MonoBehaviour, IAttack { 
    public AlertStage alertStage;
    [Range(0, 100)] public float alertLevel; // 0=peaceful, 100=alerted

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
    }

    public float movementSpeed = 5;

    [Header("Interface stuff")]
    [Range(0.01f, 1)] public float _attackSpeedSetter = 0.2f; // backing field time
    public float attackSpeed { // these lines took years off my life. please never make me write them again
        get=>_attackSpeedSetter; 
        set {_attackSpeedSetter = value;}
    }
    [SerializeField] public float attackCooldown { get; set; } = 2;
    [SerializeField] public float damage { get; set; } = 10;
    public float stunTime { get; set; } = 0;
    public Vector3 knockbackForce { get; set; } = new Vector3(0, 1, -3); // intend to scale this on damage
    [SerializeField] public bool isAttacking { get; set; } = false;
    [SerializeField] public float attackRadius { get; set; } = 1.6f;
    public void WaitForCooldown(float waitTime = 3) {
        //Debug.Log("cooldown complete");
        Invoke("ReadyToAttack", waitTime);
    }
    public void ReadyToAttack() {
        isAttacking = false;
    }
    // not needed as it only attacks player, but:
    public void AgentReenableCoroutine(NavMeshAgent agent) {
        //StartCoroutine(((IAttack)this).ReenableAgentOnGround(agent)); 
    }

    [Range(1, 100)] public float aggroSpeed = 20; // how fast alertness increments
    [Range(0, 100)] public float radius;
    [Range(0, 360)] public float angle;
        //private float maxNumberEnemies = 3; // max number of enemies to be attacking player at once
    // probably need to set this in a game settings later

    public GameObject playerRef;
    private NavMeshAgent agent;
    // make sure attack radius is not smaller than the stopping distance on navmeshagent!

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    public void Initialize()
    { }

    private void Start()
    {
        // Set interface variables
        // [Range(0.01f, 1)] public float attackSpeed = 0.2f;
        // public float attackCooldown = 2;
        // public float damage = 1;
        // public Vector3 knockbackForce = new Vector3(0, 2, -5); // intend to scale this on damage
        // [SerializeField] private bool isAttacking = false;
        // [Range(1, 50)] public float attackRadius = 1.6f;

        if (playerRef == null) {
            playerRef = GameObject.Find("Player");
        }
        StartCoroutine(FOVRoutine());
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        ((IAttack)this).CalculateKnockback();
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

    private void FieldOfViewCheck() {
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

    private void moveTowardsPlayer(Transform player) {
        //transform.LookAt(player);
        ManualLookAt(player.transform.position);

        // only chase when alerted
        if ((agent != null) && (agent.enabled == true)) {
            if (alertStage == AlertStage.Alerted) {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            } else {
                agent.SetDestination(agent.transform.position);
                agent.isStopped = true;
            }
        }
    

        // if distance between player and self is small: stop moving
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRadius) {

            // this supposedly sets the speed of the navmeshagent to zero but it doesn/t
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // and attack
            // gameObject.GetComponent<Animator>().Play("attack");
            if (alertStage == AlertStage.Alerted) {
                //if (!isAttacking) {
                // if agent is disabled, the enemy has been hit or in the stun sequence
                if (!isAttacking && agent.enabled) {
                    StartCoroutine(((IAttack)this).AttackSequence(playerRef));
                }
            }
        }
    }
    
    private void ManualLookAt(Vector3 player_pos) {
        Vector3 lookDirection = player_pos - transform.position;
        lookDirection.y = 0; // Optionally, ignore the Y axis to keep the enemy level
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
    
    // private void idle()

} // end of class