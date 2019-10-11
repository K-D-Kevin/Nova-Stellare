using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PartConnection : MonoBehaviour
{
    // Part this connection belongs to
    [SerializeField]
    private ShipParts ConnectionPart;
    public ShipParts ShipConnectionPart
    {
        get
        {
            return ConnectionPart;
        }
    }

    // Part this part connects too
    [SerializeField]
    private ShipParts AdjacentPart;
    public ShipParts ShipAdjacentPart
    {
        get
        {
            return AdjacentPart;
        }
        set
        {
            AdjacentPart = value;
        }
    }
    [SerializeField]
    private PartConnection AdjacentPartConnection;
    public PartConnection ShipAdjacentPartConnection
    {
        get
        {
            return AdjacentPartConnection;
        }
        set
        {
            AdjacentPartConnection = value;
        }
    }

    // Offset from the actual part
    [SerializeField]
    private float X_Offset = 1;
    public float Part_X_Offset
    {
        get
        { return X_Offset; }
    }
    [SerializeField]
    private float Y_Offset = 1;
    public float Part_Y_Offset
    {
        get
        { return Y_Offset; }
    }


    public bool Connected = false;

    [SerializeField]
    private bool MustConnect = false;
    public bool PartMustConnect
    {
        get
        {
            return MustConnect;
        }
    }
    // What Parts can connect to this part - NOTE: if the list size is zero, then that means ALL parts can connect
    [SerializeField]
    private List<ShipParts.Part> CanConnectTo = new List<ShipParts.Part>();
    public List<ShipParts.Part> GetCanConnectionParts
    {
        get
        {
            return CanConnectTo;
        }
    }

    public void ConnectAdjacentPart()
    {
        if (AdjacentPartConnection && AdjacentPart)
        {
            Vector3 Distance = transform.position - AdjacentPartConnection.transform.position;
            AdjacentPart.transform.position += Distance;
        }
    }
}

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
        if (MyPartConnection.ShipAdjacentPartConnection && MyPartConnection.ShipAdjacentPart)
        {
            // Only the child that is closer to the top will connect to the lower ones
            if (MyPartConnection.ShipAdjacentPart.transform.GetSiblingIndex() > MyPartConnection.transform.GetSiblingIndex())
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

