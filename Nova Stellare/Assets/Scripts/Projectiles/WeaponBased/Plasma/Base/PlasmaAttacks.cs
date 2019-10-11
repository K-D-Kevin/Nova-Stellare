using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlasmaAttacks : ShipAttacks
{
    private void Reset()
    {
        DamageType = DamageCatagory.Plasma;
    }
}
