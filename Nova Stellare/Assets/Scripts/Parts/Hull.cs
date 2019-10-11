using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : ShipParts
{
    private void Reset()
    {
        PartType = Part.Hull;
    }

    [SerializeField]
    private int SpeedReduction = 0;
    public int ShipSpeedReduction
    {
        get
        { return SpeedReduction; }
    }
}
