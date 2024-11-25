using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// i don't know the difference between system. and unityengine.random, by the way

// important consideration: this makes a separate instance of every enemy. this can get heavy on the game processing.
// should probably think about a de/re-activation radius.

public class ItemAutoSpawner : MonoBehaviour
    // please be aware that this checks the whole level lol
{
    public LayerMask itemLayer;
    public Terrain terrain;
    public float radius = 2000;
    public float checkIntervalS = 15;

    [Tooltip("how high above ground to spawn an item")]
    public float yOffset = 1f;

    // this might be better off as an array but you didn't hear that from me
    [Header("Assign items to spawn")]
    public GameObject item1;
    public int no1_max = 10;

    // copy n paste in a few places

        [Header("checkers (no need to touch these)")]
    public overworldItem no1;
    public int no1_count;

    void Start() {
        //Debug.Log("I live");
        // get component
        // if(item1 != null) {
        //     no1 = item1.GetComponentInChildren<Enemy>();
        // }

        // None selected, don't do anything
        if ((item1==null)) {
            // do nothing
            Debug.Log("no items selected to spawn!");
        } else {
            StartCoroutine(RangeCheckRoutine());
        }

    }

    private IEnumerator RangeCheckRoutine() {
        WaitForSeconds wait = new WaitForSeconds(checkIntervalS);

        while (true)
        {
            yield return wait;
            RangeCheck();
        }
    }

    private void RangeCheck() {
        Collider[] items = Physics.OverlapSphere(transform.position, radius, itemLayer);
        foreach (Collider c in items) {
                // the script technically doesn't touch anything that isn't
                // item1, item2, item3, etc
                // can add "item" tag checking here if need be
                if (c.gameObject == no1) {
                    no1_count++;
                }
            }
        // rangecheck done, call the numbers checker
        NumbersChecker();
    }

    private void NumbersChecker() {
        if (no1_count < no1_max) {
            SpawnItem(no1);
        }

        // I should probably despawn things if they're over the max so let's do that
    }

    public void SpawnItem(overworldItem item) {
        Vector3 spawnPoint = GeneratePosition(transform.position, radius);
        Instantiate(item.gameObject, spawnPoint, Quaternion.identity);
        Debug.Log("item was spawned: " + item);
    }

    // select random location
    public Vector3 GeneratePosition(Vector3 center, float radius) {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2); // random angle
        float distance = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius; // random distance

        // Calculate x and z offsets using trigonometry
        float xOffset = Mathf.Cos(angle) * distance;
        float zOffset = Mathf.Sin(angle) * distance;
        Vector3 spawnPoint = new Vector3(center.x + xOffset, center.y, center.z + zOffset);

        // Get Y value from terrain
        float terrainHeight = terrain.SampleHeight(spawnPoint);
        spawnPoint.y = terrainHeight + yOffset;

        return spawnPoint;
    }

    public void Despawn(Enemy type, int max) {
        int contestant = RussianRoulette(max);
        int index = 0;
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, itemLayer);
        foreach (Collider c in enemies) {
            if (c.gameObject == type) {
                index++;
                if(index==contestant) {
                    Debug.Log("dingdingding! enemy over max has been Despawned.");
                }
            }
        }
    }
    // i shouldnt be allowed to write method names
    public int RussianRoulette(int max) {
        int contestantNumber = (int)Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * max;
        return contestantNumber;
    }

} // end of class
