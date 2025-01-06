using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOverworldItem : OverworldItem {

    [Header("Make sure first child is Canvas if second child is Model")]
    public Vector3 meshSize;
    public MeshFilter parentMesh;

    protected override void Start() {
        selfMeshFilter = this.transform.GetChild(1).GetComponentInChildren<MeshFilter>();
        parentMesh = selfMeshFilter;
        var model = selfMeshFilter.sharedMesh;
        meshSize = new Vector3((model.bounds.size.x * transform.localScale.x),
        (model.bounds.size.y * transform.localScale.y), (model.bounds.size.z * transform.localScale.z));

        base.Start();

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
                SelfDestruct();
            }
        }
    }

    public override bool Interact(Interactor interactor) {
        WeaponsHandler handler = GameObject.FindAnyObjectByType<WeaponsHandler>();
        // find the script which equips weapon (there should only be 1)

        handler.EquipGun();
        setToDestroy = true;

        return true;
    }
}
