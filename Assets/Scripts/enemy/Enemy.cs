using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour {
    private FieldOfView fov;
    public Health healthComponent;

    [Header("Item / ScriptableObject ")]
    public Item dropsItem1;
    public Item dropsItem2;
    [Range(0,1)] public float item1Probability = 0.7f;
    public float spawnAboveOffset = 1.0f;
    private bool droppedLoot = false;

    private GameObject parent; // for spawning items

    [Header("Optional - drop prefab for spawning items in smoke")]
    public GameObject smoke;
    public GameObject itemSpawnParent;

    void Awake() {
        //fieldOfView = new FieldOfView();
        //fieldOfView.Initialize();
    }

    protected virtual void Start() {
        fov = GetComponent<FieldOfView>();
        healthComponent = GetComponentInChildren<Health>();
        itemSpawnParent = GameObject.Find("Items");
    }
    
    protected virtual void Update() {
        // Use fieldOfView for enemy movement logic

        if(healthComponent.currentHealth == 0) {
            setToDestroy = true;

            if(!droppedLoot) { 
                DropLoot();
                droppedLoot = true;
            }

            if (transform.localScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                SelfDestruct();
            }
        }
    }

    public float smoothSpeed = 2.5f;
    public bool setToDestroy = false;
    protected virtual void LateUpdate() {
        // get small
        if (setToDestroy) {
            var smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), smooth*2);
        }
        if (transform.position.y < 0) { // if you spawned below the world then destroy self
            setToDestroy = true;
        }
    }

    // void FixedUpdate() {

    // // limit rotation speed (and 'glitchiness')
    //     Rigidbody rb = GetComponent<Rigidbody>();
    // //rb.angularDrag = 5f; // Adjust as needed for smoother damping
    //     rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); // gradually reduce damping speed
    // }

    public virtual void SelfDestruct() {
        // this can overwritten in any child class by public override void selfDestruct() ...
        Debug.Log("self destructing.");
        Destroy(gameObject);
    }

    public virtual void DropLoot() {
        // calculate what drops
        float randNum = UnityEngine.Random.Range(0f, 1f);
        //Debug.Log(randNum);

        // Make a smoke
        //GameObject smoke = GameObject.Find("PuffOfSmoke");
        //Debug.Log(smoke);
        try {
            GameObject smokePuff = Instantiate(smoke, this.transform);
            SmokePuff smokeScript = smokePuff.GetComponent<SmokePuff>();
            // call functions on smokeScript here
        } catch {
            Debug.Log("failed to play smoke animation");
        }


        // drop it
        var spawnOffset =  new Vector3(0.0f, spawnAboveOffset, 0.0f);
        if (randNum >= item1Probability) {
            dropsItem1.Spawn(transform.position + spawnOffset, transform.rotation, itemSpawnParent.transform);
        } else {
            dropsItem2.Spawn(transform.position + spawnOffset, transform.rotation, itemSpawnParent.transform);
        }
        // the last argument is the "folder" the gameobject goes into

    }
}