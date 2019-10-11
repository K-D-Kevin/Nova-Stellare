using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : ShipParts
{
    private void Reset()
    {
        PartType = Part.Cockpit;
    }
    

    [SerializeField]
    private float ShieldRegenMultipler = 1;
    public float ShipShieldRegenMultipler
    {
        get
        { return ShieldRegenMultipler; }
    }
}
