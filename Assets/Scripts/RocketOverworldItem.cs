using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RocketOverworldItem : OverworldItem {

    public static event Action OnCollected;
    public static int total;

    [Header("Make sure first child is Canvas if second child is Model")]
    public Vector3 meshSize;
    //public MeshCollider meshCollider;

    void Awake() => total++;

    protected override void Start() {
        selfMeshFilter = this.transform.GetChild(1).GetComponentInChildren<MeshFilter>();
        var model = selfMeshFilter.sharedMesh;
        meshSize = new Vector3((model.bounds.size.x * transform.localScale.x),
        (model.bounds.size.y * transform.localScale.y), (model.bounds.size.z * transform.localScale.z));
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.WakeUp();
        }

        base.Start();
        playerCam = Camera.main;
        playerTransform = playerCam.transform;

    }

    protected override void LateUpdate() {
        base.LateUpdate();
    }

    protected override void Update() {
        //base.Update();

        if (this == null) return;
        
        // rewrite of update() to use autosetlabel from this class
        playerTransform = playerCam.transform;
        ((IInteractable)this).UpdateVisibility();
        ((IInteractable)this).SetLabelPosition(meshSize.x, meshSize.y+1, meshSize.z);
        if (setToDestroy) {
            if (transform.localScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                //SelfDestruct();
                Destroy(gameObject);
            }
        }
    }

    public override bool Interact(Interactor interactor) {
        
        Debug.Log("interacted with rocket");
        OnCollected?.Invoke();

        setToDestroy = true;
        return true;
    }
}
