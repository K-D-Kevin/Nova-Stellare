using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipAttacks : MonoBehaviour
{
    public enum DamageCatagory
    {
        Collision = 0,
        Bullet = 1,
        Plasma = 2,
        Lasers = 3,
        Explosive = 4,
        Other = 5,
        Etc,
    }

    // Part Type
    protected DamageCatagory DamageType = DamageCatagory.Other;
    public DamageCatagory ShipDamageType
    {
        get
        { return DamageType; }
    }

    [SerializeField]
    private int Damage = 1;
    public int AttackDamage
    {
        get
        { return Damage; }
    }

    // On Collision
    public abstract void AttackOnCollisionEnter(Collision2D Col);

    public abstract void AttackOnCollisionStay(Collision2D Col);

    public abstract void AttackOnCollisionExit(Collision2D Col);

    // On Trigger
    public abstract void AttackOnTriggerEnter(Collider2D Col);

    public abstract void AttackOnTriggerStay(Collider2D Col);

    public abstract void AttackOnTriggerExit(Collider2D Col);
}
