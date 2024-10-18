using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Alert system. A number increases the longer the target stays in the fov zone.
// (and decreases when outside of): maxed/being alerted means enemy becomes hostile
public enum AlertStage
{
    Peaceful,
    Aware,
    Alerted
}

public class enemy : MonoBehaviour
{
    public float fov;
    [Range(0, 360)] public float fovAngle; // angle of cone of vision, degrees

    public AlertStage alertStage;
    [Range(0, 100)] public float alertLevel; // 0=peaceful, 100=alerted

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
    }

    private void onDrawGizmos() // draw fov - wont work in 2023.1
    {
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawSolidDisc(transform.position, transform.up, fov); // drawsolidarc
    }

    private void Update()
    {
        bool playerInFov = false;
        // get array of things colliding with fov sphere
        Collider[] targetsInFOV = Physics.OverlapSphere(
            transform.position, fov);
        foreach (Collider c in targetsInFOV) // if there exists an object with player tag in the list of colliders
        {
            if (c.CompareTag("Player"))
                // calc angle
                float signedAngle = Vector3.Angle(
                    transform.forward,
                    c.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle / 2)
                    playerInFOV = true;
                break;
        }
        _UpdateAlertState(playerInFov);
    }

    private void _UpdateAlertState(bool playerInFOV)
    {
        switch(alertStage)
        {
            case AlertStage.Peaceful:
                if (playerInFOV)
                    alertStage = AlertStage.Aware;
                break;

            case AlertStage.Aware: // increment if in fov, decrement if not
                if (playerInFOV)
                    alertLevel++;
                    if (alertLevel >= 100)
                        alertStage = AlertStage.Alerted;
                else
                    alertLevel--;
                    if (alertLevel <= 0)
                        alertStage = AlertStage.Peaceful;
                break;

            case AlertStage.Alerted: // decrement if not in fov
                if (!playerInFOV)
                    alertStage = AlertStage.Aware;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class FieldofView : MonoBehavior
{
    public float radius;
    [Range(0, 360)] public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator FOVRoutine() // core routine to reduce number of calls per frame (for performance)
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        // array of things that collide with your fov cone
        
        if (rangeChecks.length != 0)
        {
            Transform target = rangeChecks[0].transform; // this only gets the first in rangecheck
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // check for angle
            if ((Vector3.Angle(transform.forward, directionToTarget)) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // limit raycast distance to whenever it hits something (vision is blocked by walls)
                if (!(Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false; // player is not within radius
        }
        else (canSeePlayer)
            canSeePlayer = false;
            // if check failed and you were previously viewing, you are no longer able to view player
    }


}
