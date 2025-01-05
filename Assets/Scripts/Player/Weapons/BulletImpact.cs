using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour {
    void Start() {
        // check that length of particle system playback is less than dieAfter.
        StartCoroutine(DieAfter());
    }

    public IEnumerator DieAfter(float s = 2.0f) {
        yield return new WaitForSeconds(s);
        Destroy(this.gameObject);
    }
}
