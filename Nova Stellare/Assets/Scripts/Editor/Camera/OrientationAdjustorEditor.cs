using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrientationAdjustor))]
public class OrientationAdjustorEditor : Editor
{
    OrientationAdjustor MyOrientation;
    private float Angle = 0;
    public void OnEnable()
    {
        MyOrientation = (OrientationAdjustor)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        if (GUILayout.Button("Rotate"))
        {
            MyOrientation.RotateTransforms(Angle);
            if (Angle == 0)
            {
                Angle = 90;
            }
            else if (Angle == 90)
            {
                Angle = -90;
            }
            else if (Angle == -90)
            {
                Angle = 90;
            }
        }
    }
}
