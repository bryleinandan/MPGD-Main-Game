// debug enemy fov.
// compsci-3 interactive / https://www.youtube.com/watch?v=j1-OyLo77ss

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(FieldOfView))]

public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        Debug.Log("im running");
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

    }
}

