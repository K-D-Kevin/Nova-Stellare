using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LasersAttacks : ShipAttacks
{

    private void Reset()
    {
        DamageType = DamageCatagory.Lasers;
    }
}
