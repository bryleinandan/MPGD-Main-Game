using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePuff : MonoBehaviour
{
    private float destroyAfter;
    public ParticleSystem particles;

    // destroy self after the particle times out

    void Start() {
        particles = GetComponent<ParticleSystem>();
        destroyAfter = particles.main.duration + 1.0f;
        StartCoroutine(Timeout());
        Debug.Log("destroying smoke");
        Destroy(gameObject);
    }

    void Update() {
        
    }

    private IEnumerator Timeout() {
        WaitForSeconds wait = new WaitForSeconds(destroyAfter);

        while (true) {
            yield return wait;
        }
    }
}
