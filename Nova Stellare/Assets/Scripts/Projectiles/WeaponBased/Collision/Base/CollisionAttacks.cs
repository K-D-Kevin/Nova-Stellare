using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionAttacks : ShipAttacks
{
    private void Reset()
    {
        DamageType = DamageCatagory.Collision;
    }
}
