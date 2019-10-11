using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : ShipParts
{
    private void Reset()
    {
        PartType = Part.Thruster;
    }

    [SerializeField]
    private int Thrust = 1;
    public int PartThrust
    {
        get
        { return Thrust; }
    }
}
