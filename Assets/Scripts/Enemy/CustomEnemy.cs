using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnemy : Enemy {
    //[Header("Make sure second child is Model")]

    protected override void Start() {

        // get mesh from child and assign it to self
        MeshFilter childMeshFilter = this.transform.GetChild(1).GetComponentInChildren<MeshFilter>();
        MeshFilter thisMeshFilter = GetComponent<MeshFilter>();
        MeshCollider thisMeshCollider = null;

        // if this object uses the mesh as a collider, set it too
        if (gameObject.TryGetComponent(out MeshCollider mc)) { thisMeshCollider = mc; }
        if (thisMeshCollider != null) {
            thisMeshCollider.sharedMesh = childMeshFilter.sharedMesh;
        }

        thisMeshFilter.sharedMesh = childMeshFilter.sharedMesh;

        base.Start();

    }

    protected override void Update() { base.Update(); }

    protected override void LateUpdate() { base.LateUpdate(); }

}
