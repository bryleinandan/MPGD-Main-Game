// // debug enemy fov.
// // compsci-3 interactive / https://www.youtube.com/watch?v=j1-OyLo77ss

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// [CustomEditor(typeof(EnemyAutoSpawner))]

// // This script only runs within unity's editor.
// // but it lets you view and edit the enemy field of view. so that's helpful.
// // optimal debugging setup is to have scene and game view side by side as this editor code
// // will not check for player presence during setup

// public class FOVEditor : Editor
// {
//     private void OnSceneGUI()
//     {
//         FieldOfView fov = (FieldOfView)target;

//         Color c = Color.green;

//         //Handles.color = new Color(0, 1, 0, 0.3f);
//         if (fov.alertStage == AlertStage.Aware)
//         {
//             c = Color.Lerp(Color.green, Color.red, fov.alertLevel / 100f);
//         }
//         else if (fov.alertStage == AlertStage.Alerted)
//             c = Color.red;
        
//         // Draw circle -- no longer used as arc is calculated and drawn
//         //Handles.color = new Color(c.r, c.g, c.b, 0.3f);
//         //Handles.DrawSolidDisc(fov.transform.position, fov.transform.up, fov.radius); // draw a circle there's the earth

//         // make a handle/gizmo to edit the fov in the editor
//         Handles.color = c;
//         fov.radius = Handles.ScaleValueHandle(fov.radius, fov.transform.position + fov.transform.forward * fov.radius,
//             fov.transform.rotation, 3, Handles.SphereHandleCap, 1);


//         // Draw the fov arc
//         Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
//         Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

//         Handles.color = Color.white;
//         Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.radius);
//         Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.radius);

//         Handles.color = new Color(c.r, c.g, c.b, 0.3f);
//         Handles.DrawSolidArc(fov.transform.position, Vector3.up, viewAngle1, fov.angle, fov.radius); // drawsolidarc

//     }

//     // don't ask. I don't know. viewing angle is split in half because view both halves or soemthing
//     private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
//     {
//         angleInDegrees += eulerY;
//         return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//     }

// }

