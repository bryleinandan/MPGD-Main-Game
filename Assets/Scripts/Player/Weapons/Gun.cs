using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, IAttack {

    // interface things
    public float attackSpeed { get; set; }
    public float attackCooldown { get; set; } = 2;
    public float damage { get; set; } = 20;
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

    public float range = 50f;
    public Camera playerCam;
    //public InputAction Fire;
    public PlayerInput playerInput;
    public LayerMask targetLayer;

    void Start() {
        CalculateKnockback();

        if (playerCam == null) {
            playerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        }

        if (playerInput == null) {
            GameObject player = GameObject.Find("Player");
            playerInput = player.GetComponent<PlayerInput>();
        }

        // if (targetLayer == null) {
        //     int LayerIgnoreRaycast = LayerMask.NameToLayer("Attackable");
        // }
    }

    public void CalculateKnockback() { // basically override
        float x = 0;
        float y = damage*0.2f;
        float z = -(damage*0.4f);
        knockbackForce = new Vector3(x, y, z);
    }

    void Update()
    {
        if (playerInput.actions["Fire"].WasPerformedThisFrame()) {
            //Debug.Log("fire was performed this frame");
            Shoot();
        }

        void Shoot() {
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range)) {
                //Debug.Log(hit.transform.name);

                GameObject hitObj = hit.transform.gameObject;

                // if on target layer: do iattack

                //if (target.layer == LayerMask.NameToLayer(targetLayer.name)) {
                // had some issues comparing target.layer -> number with targetLayer -> LayerMask

                // Check if the hit object's layer is in the LayerMask
                if ((targetLayer.value & (1 << hitObj.layer)) != 0) {
                    Debug.Log("targetlayer matched");
                    ((IAttack)this).DealDamage(hitObj.gameObject);
                    ((IAttack)this).ApplyKnockback(hitObj.gameObject);
                }
            }
        }
    }
}
