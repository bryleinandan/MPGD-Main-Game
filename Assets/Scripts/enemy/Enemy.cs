using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    //public FieldOfView fieldOfView; 
    private FieldOfView fov;
    private Health healthComponent;

    [Header("Item / ScriptableObject ")]
    public Item dropsItem1;
    public Item dropsItem2;
    [Range(0,1)] public float item1Probability = 0.7f;
    public float spawnAboveOffset = 1.0f;

    private GameObject parent;

    void Awake() {
        //fieldOfView = new FieldOfView();
        //fieldOfView.Initialize();
    }

    void Start() {
        fov = GetComponent<FieldOfView>();
        healthComponent = GetComponentInChildren<Health>();
    }
    
    void Update() {
        // Use fieldOfView for enemy logic

        if(healthComponent.currentHealth == 0) {
            setToDestroy = true;

            if (transform.localScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                DropLoot();
                SelfDestruct();
            }
        }
    }

    public float smoothSpeed = 2.5f;
    public bool setToDestroy = false;
    void LateUpdate() {
        // get small
        if (setToDestroy) {
            var smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), smooth*2);
        }
        if (transform.position.y < 0) { // if you spawned below the world then destroy self
            setToDestroy = true;
        }
    }

    public virtual void SelfDestruct() {
        // this can overwritten in any child class by public override void selfDestruct() ...
        Debug.Log("self destructing.");
        Destroy(gameObject);
    }

    public virtual void DropLoot() {
        // calculate what drops
        float randNum = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(randNum);

        // drop it
        var spawnOffset =  new Vector3(0.0f, spawnAboveOffset, 0.0f);
        parent = GameObject.Find("Items"); // item parent
        if (randNum >= item1Probability) {
            dropsItem1.Spawn(transform.position + spawnOffset, transform.rotation);
        } else {
            dropsItem2.Spawn(transform.position + spawnOffset, transform.rotation);
        }

    }
}