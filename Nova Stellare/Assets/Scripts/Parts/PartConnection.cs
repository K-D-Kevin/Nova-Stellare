using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
