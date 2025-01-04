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
    public float attackRadius { get; set; } = 3f;
    public void WaitForCooldown(float waitTime = 0) {
        Invoke("ReadyToAttack", waitTime);
    }
    public void ReadyToAttack() {
        isAttacking = false;
    }

    [Range(0.01f,50f)]public float range = 2.0f;
    public Camera playerCam;
    //public InputAction Fire;
    public PlayerInput playerInput;
    public LayerMask targetLayer;
    private ParticleSystem muzzleFlash;

    void Start() {
        CalculateKnockback();
        attackRadius = range;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();

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
        float y = damage*0.07f;
        float z = -(damage*0.2f);
        knockbackForce = new Vector3(x, y, z);
    }

    void Update()
    {
        if (playerInput.actions["Fire"].WasPerformedThisFrame()) {
            //Debug.Log("fire was performed this frame");
            Shoot();
        }

        void Shoot() {
            muzzleFlash.Play();

            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, attackRadius)) {
                //Debug.Log(hit.transform.name);

                //play bullet hit at hit

                GameObject hitObj = hit.transform.gameObject;

                // if on target layer: do iattack

                //if (target.layer == LayerMask.NameToLayer(targetLayer.name)) {
                // had some issues comparing target.layer -> number with targetLayer -> LayerMask

                // Check if the hit object's layer is in the LayerMask
                if ((targetLayer.value & (1 << hitObj.layer)) != 0) {
                    //Debug.Log("targetlayer matched");
                    ((IAttack)this).DealDamage(hitObj.gameObject);
                    ((IAttack)this).ApplyKnockback(hitObj.gameObject);
                }
            }
        }
    }
}
