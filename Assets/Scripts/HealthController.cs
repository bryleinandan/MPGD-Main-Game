using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public HealthSystem healthBar;
    public Vector3 offset = new Vector3(0, 0f, 0); // offset health bar above self
    [SerializeField] private bool healthShowing = false;
    [SerializeField] private Vector3 originalScale = new Vector3(1,1,1);

    void Start() {
        if(healthBar == null) {
            healthBar = GetComponentInChildren<HealthSystem>();
        }
        originalScale = healthBar.transform.localScale;
    }
    
    void Update() {
    //     // health bar follow the object
    //     healthBar.transform.position = transform.position + offset;
    //     //healthBar.transform.LookAt(Camera.main.transform); // Face the camera
        
        
        // Show health if <100%
        if(healthShowing) {
            healthBar.transform.localScale = originalScale;
        } else {
            healthBar.transform.localScale = new Vector3 (0, 0, 0);
        }
    }

    void LateUpdateLabelRotation() {
        // needs direct reference to playercam, otherwise won't rotate
        //healthBar.transform.rotation = Camera.main.transform.rotation;
    }

    public void TakeDamage(float dmg) {
        healthShowing = true;
        healthBar.GetComponent<Health>().TakeDamage((int)dmg);
    }
}
