using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public HealthSystem healthBar;
    public Vector3 offset = new Vector3(0, 0.2f, 0); // offset health bar above self

    void Start() {
        if(healthBar == null) {
            healthBar = GetComponentInChildren<HealthSystem>();
        }
    }
    
    void Update() {

        // health bar follow the object
        healthBar.transform.position = transform.position + offset;
        //healthBar.transform.LookAt(Camera.main.transform); // Face the camera
    }

     void LateUpdateLabelRotation() {
        healthBar.transform.rotation = Camera.main.transform.rotation;
    }

    public void TakeDamage(float dmg) {
        healthBar.GetComponent<Health>().TakeDamage((int)dmg);
    }
}
