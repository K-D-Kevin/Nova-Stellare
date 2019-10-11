using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ShipParts
{
    private void Reset()
    {
        PartType = Part.Weapon;
    }

    [SerializeField]
    private int Thrust = 0;
    public int PartThrust
    {
        get
        { return Thrust; }
    }

    [SerializeField]
    private Transform ProjectileSpawn;

    [SerializeField]
    private ShipAttacks WeaponAttackPrefab;

    // Launch Attack
    public void Fire()
    {
        ShipAttacks Temp = Instantiate(WeaponAttackPrefab, ProjectileSpawn);
        Temp.transform.parent = null;
    }
}
