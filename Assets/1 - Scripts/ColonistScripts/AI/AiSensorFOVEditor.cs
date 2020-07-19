using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ProjectColoni
{
    [CustomEditor (typeof (AiSensors))]
    public class FieldOfViewEditor : Editor
    {
        
        private void OnSceneGUI()
        {
            var fow = (AiSensors)target;
            Handles.color = new Color(0.16f, 1f, 0.47f);
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.radius);
            Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
            Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.radius);
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.radius);

            Handles.color = Color.red;
            // foreach (var visibleTargets in fow.visibleTargets)
            // {
            //     //Handles.DrawLine(fow.transform.position + new Vector3(0, 2.8f, 0), visibleTargets.position + new Vector3(0, 2.8f, 0));
            //     //Handles.DrawLine(fow.transform.position, visibleTargets.position);
            // }
        }
    }
}
#endif

