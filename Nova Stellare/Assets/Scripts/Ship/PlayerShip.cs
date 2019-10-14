using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerShip : MonoBehaviour
{

    // Ship Components
    // All Parts
    private ShipParts[] Parts;
    public ShipParts[] ShipPartList
    {
        get
        {
            return Parts;
        }
    }
    // Cockpit
    private Cockpit ShipCockpitPart;
    public Cockpit ShipCockpit
    {
        get
        {
            return ShipCockpitPart;
        }
    }
    // Hull
    private List<Hull> HullParts = new List<Hull>();
    public List<Hull> HullPartList
    {
        get
        {
            return HullParts;
        }
    }
    // Wing
    private List<Wing> WingParts = new List<Wing>();
    public List<Wing> WingPartList
    {
        get
        {
            return WingParts;
        }
    }
    // Thrusters
    private List<Thruster> ThrusterParts = new List<Thruster>();
    public List<Thruster> ThrusterPartList
    {
        get
        {
            return ThrusterParts;
        }
    }
    // Weapons
    // Weapons All
    private List<Weapon> WeaponParts = new List<Weapon>();
    public List<Weapon> WeaponPartList
    {
        get
        {
            return WeaponParts;
        }
    }
    // Type Of Fire "How the player makes the weapon attack"
    // Hold
    private List<Weapon> HoldWeapons = new List<Weapon>();
    public List<Weapon> HoldWeaponList
    {
        get
        {
            return HoldWeapons;
        }
    }
    // On Down
    private List<Weapon> OnDownWeapons = new List<Weapon>();
    public List<Weapon> OnDownWeaponList
    {
        get
        {
            return OnDownWeapons;
        }
    }
    // On Up
    private List<Weapon> OnUpWeapons = new List<Weapon>();
    public List<Weapon> OnUpWeaponList
    {
        get
        {
            return OnUpWeapons;
        }
    }
    // When not touching
    private List<Weapon> NoTouchWeapons = new List<Weapon>();
    public List<Weapon> NoTouchWeaponList
    {
        get
        {
            return NoTouchWeapons;
        }
    }
    // Contact Weapons
    private List<Weapon> ContactWeapons = new List<Weapon>();
    public List<Weapon> ContactWeaponList
    {
        get
        {
            return ContactWeapons;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        Parts = GetComponentsInChildren<ShipParts>();
        // Set all the part lists
        SetVariables();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Application.isEditor)
        {
            MouseInput();
        }
        else
        {
            TouchInput();
        }
    }

    public void FireWeapons(Weapon.TypeOfFire Mode)
    {
        if (Mode == Weapon.TypeOfFire.Hold)
        {
            foreach (Weapon weap in HoldWeapons)
            {
                weap.Fire();
            }
        }
        else if (Mode == Weapon.TypeOfFire.OnDown)
        {
            foreach (Weapon weap in OnDownWeapons)
            {
                weap.Fire();
            }
        }
        else if (Mode == Weapon.TypeOfFire.OnUp)
        {
            foreach (Weapon weap in OnUpWeapons)
            {
                weap.Fire();
            }
        }
        else if (Mode == Weapon.TypeOfFire.NoTouch)
        {
            foreach (Weapon weap in NoTouchWeapons)
            {
                weap.Fire();
            }
        }
    }

    public void SetVariables()
    {
        // Set all the part lists
        foreach (ShipParts part in Parts)
        {
            if (part.GetComponent<Cockpit>() != null)
            {
                ShipCockpitPart = part.GetComponent<Cockpit>();
            }
            else if (part.GetComponent<Hull>() != null)
            {
                HullPartList.Add(part.GetComponent<Hull>());
            }
            else if (part.GetComponent<Wing>() != null)
            {
                WingPartList.Add(part.GetComponent<Wing>());
            }
            else if (part.GetComponent<Thruster>() != null)
            {
                ThrusterPartList.Add(part.GetComponent<Thruster>());
            }
            else if (part.GetComponent<Weapon>() != null)
            {
                WeaponPartList.Add(part.GetComponent<Weapon>());
            }
        }
        // Set weapon lists
        foreach (Weapon weap in WeaponPartList)
        {
            if (weap.FiringMode == Weapon.TypeOfFire.Hold)
            {
                HoldWeapons.Add(weap);
            }
            else if (weap.FiringMode == Weapon.TypeOfFire.OnDown)
            {
                OnDownWeapons.Add(weap);
            }
            else if (weap.FiringMode == Weapon.TypeOfFire.OnUp)
            {
                OnUpWeapons.Add(weap);
            }
            else if (weap.FiringMode == Weapon.TypeOfFire.NoTouch)
            {
                NoTouchWeapons.Add(weap);
            }
            else if (weap.FiringMode == Weapon.TypeOfFire.Contact)
            {
                ContactWeapons.Add(weap);
            }
        }
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {

            Vector2 Location = Input.GetTouch(0).position;
            // OnDown
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Began");
                if (OnDownWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnDown);
            }
            // Hold
            else if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Hold");
                if (HoldWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.Hold);
            }
            // On Up
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ended");
                if (OnUpWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnUp);
            }
        }
        // When not touching
        else
        {
            if (NoTouchWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.NoTouch);
        }
    }

    private void DecideTouchDecision(int touch)
    {

    }
    private void MouseInput()
    {
        // Right Mouse Buttons
        // OnDown
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Began");
            if (OnDownWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.OnDown);
        }
        // Hold
        else if (Input.GetMouseButton(1))
        {
            FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Hold");
            if (HoldWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.Hold);
        }
        // On Up
        else if (Input.GetMouseButtonUp(1))
        {
            FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ended");
            if (OnUpWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.OnUp);
        }
        // When not touching
        else
        {
            if (NoTouchWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.NoTouch);
        }
    }
}