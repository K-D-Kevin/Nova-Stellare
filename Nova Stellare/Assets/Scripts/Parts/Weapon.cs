using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ShipParts
{
    // How the player launches an attack
    public enum TypeOfFire
    {
        OnDown = 0,
        OnUp = 1,
        Hold = 2,
        NoTouch = 3,
        Contact = 4,
    }
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
    [Range(0, Mathf.Infinity)]
    private float ReloadTime = .1f;
    public float PartReloadTime
    {
        get
        { return ReloadTime; }
    }
    private float CurrentReloadingTime = 0;
    public float PartCurrentReloadingTime
    {
        get
        { return CurrentReloadingTime; }
    }
    private bool Reloading = false;
    public bool WeaponReloading
    {
        get
        {
            return Reloading;
        }
    }


    [SerializeField]
    private TypeOfFire FireMode = TypeOfFire.Hold;
    public TypeOfFire FiringMode
    {
        get
        { return FireMode; }
    }

    [SerializeField]
    private Transform ProjectileSpawn;

    [SerializeField]
    private ShipAttacks WeaponAttackPrefab;

    // Launch Attack
    public void Fire()
    {
        if (!Reloading)
        {
            ShipAttacks Temp = Instantiate(WeaponAttackPrefab, ProjectileSpawn);
            Temp.transform.parent = null;
            Reloading = true;
            StartCoroutine("IE_Fire");
        }
    }

    private IEnumerator IE_Fire()
    {
        while(CurrentReloadingTime < ReloadTime)
        {
            CurrentReloadingTime += Time.deltaTime;
            yield return null;
        }

        Reloading = false;
        CurrentReloadingTime = 0;
    }
}
