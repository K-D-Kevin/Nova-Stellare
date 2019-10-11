using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAttacks : ShipAttacks
{
    private void Reset()
    {
        DamageType = DamageCatagory.Bullet;
    }
}
