using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : ShipParts
{
    private void Reset()
    {
        PartType = Part.Wing;
    }

    [SerializeField]
    private int SpeedIncrease = 1;
    public int ShipSpeedIncrease
    {
        get
        { return SpeedIncrease; }
    }

    [SerializeField]
    private int AgilityIncrease = 1;
    public int ShipAgilityIncrease
    {
        get
        { return AgilityIncrease; }
    }
}
