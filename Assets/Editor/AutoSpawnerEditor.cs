using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(EnemyAutoSpawner))]

// REMEMBER, in order to see it, you need to have selected spawner in the tree view

public class AutoSpawnerEditor : Editor {
    private void OnSceneGUI() {
        EnemyAutoSpawner spawner = (EnemyAutoSpawner)target;

        // make a handle/gizmo to edit the fov in the editor
        Handles.color = new Color(0.5f, 0.2f, 0.7f, 0.3f); // i just picked a colour
        Handles.DrawSolidDisc(spawner.transform.position, spawner.transform.up, spawner.radius);
        // or wiredisk!

        // draw circle handle (not that you'll see it)
        spawner.radius = Handles.ScaleValueHandle(spawner.radius, spawner.transform.position + spawner.transform.forward 
        * spawner.radius, spawner.transform.rotation, 10, Handles.SphereHandleCap, 1);
    }

}

