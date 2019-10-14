using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartConnection))]
public class PartConnectionEditor : Editor
{
    PartConnection MyPartConnection;
    public void OnEnable()
    {
        MyPartConnection = (PartConnection)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MyPartConnection.transform.localPosition = new Vector3(MyPartConnection.Part_X_Offset, MyPartConnection.Part_Y_Offset, 0);
        if (MyPartConnection.ShipAdjacentPartConnection && MyPartConnection.ShipAdjacentPart && MyPartConnection.ShipConnectionPart)
        {
            // Only the child that is closer to the top will connect to the lower ones
            if (MyPartConnection.ShipAdjacentPart.transform.GetSiblingIndex() > MyPartConnection.ShipConnectionPart.transform.GetSiblingIndex())
            {
                MyPartConnection.ConnectAdjacentPart();
                if (GUILayout.Button("Connect"))
                {
                    MyPartConnection.ConnectAdjacentPart();
                }
            }
            else if (GUILayout.Button("Connect Anyway"))
            {
                MyPartConnection.ConnectAdjacentPart();
            }
        }
    }
}
