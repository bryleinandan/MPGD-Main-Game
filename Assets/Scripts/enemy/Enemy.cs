using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //public FieldOfView fieldOfView; 
    private FieldOfView fov;
    private Health healthComponent;

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
    }

    public virtual void SelfDestruct() {
        // this can overwritten in any child class by public override void selfDestruct() ...
        Debug.Log("self destructing.");
        Destroy(gameObject);
    }
}