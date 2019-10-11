using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipParts : MonoBehaviour
{
    public enum Part
    {
        Cockpit = 0,
        Wing = 1,
        Hull = 2,
        Weapon = 3,
        Thruster = 4,
        Other = 5,
        Etc,
    }

    // Part Type
    protected Part PartType = Part.Other;
    public Part ShipPart
    {
        get
        { return PartType; }
    }

    // Part Adjustable Variables
    // Health components of ship
    [SerializeField]
    private int HealthOfPart = 1;
    public int PartHealth
    {
        get
        { return HealthOfPart; }
    }

    [SerializeField]
    private int MaxHealthOfPart = 1;
    public int MaxPartHealth
    {
        get
        { return MaxHealthOfPart; }
    }

    [SerializeField]
    private int ShieldUnits = 1;
    public int AmountOfShields
    {
        get
        { return ShieldUnits; }
    }

    [SerializeField]
    private int MaxShieldUnits = 1;
    public int MaxAmountOfShields
    {
        get
        { return MaxShieldUnits; }
    }

    [SerializeField]
    private float DelayForShieldDelay = 3;
    public float ShieldRepairDelay
    {
        get
        { return DelayForShieldDelay; }
    }

    // Damage on collision
    [SerializeField]
    private int DamageDeltFromCollision = 0;
    public int CollisionDamage
    {
        get
        { return DamageDeltFromCollision; }
    }

    // Damage Resistance from varias sources
    [SerializeField]
    private int DamageResistanceFromCollision = 1;
    public int CollisionDamageResistance
    {
        get
        { return DamageResistanceFromCollision; }
    }

    [SerializeField]
    private int DamageResistanceFromBullets = 1;
    public int BulletDamageResistance
    {
        get
        { return DamageResistanceFromBullets; }
    }

    [SerializeField]
    private int DamageResistanceFromPlasma = 1;
    public int PlasmaDamageResistance
    {
        get
        { return DamageResistanceFromPlasma; }
    }

    [SerializeField]
    private int DamageResistanceFromLasers = 1;
    public int LaserDamageResistance
    {
        get
        { return DamageResistanceFromLasers; }
    }

    [SerializeField]
    private int DamageResistanceFromExplosives = 1;
    public int ExplosiveDamageResistance
    {
        get
        { return DamageResistanceFromExplosives; }
    }

    // Connect Parts
    [SerializeField]
    private int ConnectionSpots = 1;
    public int ShipConnectionAmount
    {
        get
        { return ConnectionSpots; }
    }
    [SerializeField]
    private PartConnection[] PartConnectionList;
    public PartConnection[] ShipConnectionList
    {
        get
        { return PartConnectionList; }
    }

    // Part Colors
    [SerializeField]
    private Color PrimaryColor;
    public Color PartPrimaryColor
    {
        get
        {
            return PrimaryColor;
        }
        set
        {
            PrimaryColor = value;
        }
    }
    [SerializeField]
    private Color SecondaryColor;
    public Color PartSecondaryColor
    {
        get
        {
            return SecondaryColor;
        }
        set
        {
            SecondaryColor = value;
        }
    }
    [SerializeField]
    private Color TertiaryColor;
    public Color PartTertiaryColor
    {
        get
        {
            return TertiaryColor;
        }
        set
        {
            TertiaryColor = value;
        }
    }

    // Unadjustable Variables
    private bool ShipRecentlyDamaged = false;
    public bool RecentlyDamaged
    {
        get; set;
    }

    public void Damage(int dmg = 1)
    {
        if (ShieldUnits == 0)
        {
            HealthOfPart -= dmg;
            if (HealthOfPart < 0)
                HealthOfPart = 0;
        }
        else
        {
            ShieldUnits -= dmg;
            if (ShieldUnits < 0)
            {
                HealthOfPart -= ShieldUnits;
                ShieldUnits = 0;
                if (HealthOfPart < 0)
                    HealthOfPart = 0;
            }
        }
    }

    public void RepairHealth(int rpr = 1)
    {
        HealthOfPart += rpr;
        if (HealthOfPart > MaxHealthOfPart)
            HealthOfPart = MaxHealthOfPart;
    }

    public void RepairShield(int rpr = 1)
    {
        HealthOfPart += rpr;
        if (HealthOfPart > MaxHealthOfPart)
            HealthOfPart = MaxHealthOfPart;
    }

    public IEnumerator RepairShields(float AdditionalRepairTime = 0, float RepairMultiplier = 1)
    {
        float TimeForRepair = (ShieldRepairDelay + MaxShieldUnits + AdditionalRepairTime) * RepairMultiplier;
        int LastTimeInt = Mathf.RoundToInt(ShieldRepairDelay);
        while (TimeForRepair > 0 && ShieldUnits < MaxShieldUnits && !RecentlyDamaged)
        {
            TimeForRepair -= Time.deltaTime;

            if (TimeForRepair < ShieldRepairDelay)
            {
                if (LastTimeInt > Mathf.CeilToInt(TimeForRepair))
                {
                    LastTimeInt = Mathf.CeilToInt(TimeForRepair);
                    RepairShield();
                }
            }

            yield return null;
        }
        ShieldUnits = MaxShieldUnits;

        if (RecentlyDamaged)
            RecentlyDamaged = false;
    }


}