using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveAttacks : ShipAttacks
{
    private void Reset()
    {
        DamageType = DamageCatagory.Explosive;
    }
}
