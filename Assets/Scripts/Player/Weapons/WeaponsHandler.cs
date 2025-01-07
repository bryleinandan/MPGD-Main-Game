using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour {
    
    public GameObject crosshair;
    public Camera playerCamera;
    public GameObject WeaponHolderPrefab;
    public bool gunIsEquipped = false;
    private bool gunStateWasChanged = false; // set true on update

    void Start() {
        
        if (crosshair == null) {
            crosshair = GameObject.Find("Crosshair");
        }
        crosshair.transform.localScale = new Vector3(0,0,0);

        if (playerCamera == null) {
            playerCamera = Camera.main;
        }
    }

    void Update() {
        if (gunStateWasChanged) {
            if (!gunIsEquipped) {
                crosshair.transform.localScale = new Vector3 (0, 0, 0);
                WeaponHolder holder = playerCamera.GetComponentInChildren<WeaponHolder>();
                Destroy(holder.gameObject);
            } else {
                Instantiate(WeaponHolderPrefab, playerCamera.transform);
                crosshair.transform.localScale = new Vector3 (1, 1, 1);
            }
            gunStateWasChanged = false;
        }
        
    }

    public void EquipGun() {
        gunIsEquipped = true;
        gunStateWasChanged = true;
    }

    public void UnequipGun() {
        gunIsEquipped = false;
        gunStateWasChanged = true;
    }
    
}
