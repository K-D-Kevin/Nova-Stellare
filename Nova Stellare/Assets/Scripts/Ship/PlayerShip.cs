using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private bool NoTouchesRight = true;
    // Contact Weapons
    private List<Weapon> ContactWeapons = new List<Weapon>();
    public List<Weapon> ContactWeaponList
    {
        get
        {
            return ContactWeapons;
        }
    }
    // Ship transform
    private Transform Mytransform;
    // Ship Attacks Transform
    private Transform WeaponFire;

    // Touch detection and control detection
    [SerializeField]
    private GraphicRaycaster JoystickHitRaycastor;
    private PointerEventData JoystickEventData;
    private EventSystem SceneEventSystem;
    private Vector2 ScreenLocation;
    public Vector2 TouchScreenLocation { get { return ScreenLocation; } }
    private int FingerIdFollow = -1; // -1 means no finger is getting followed
    private Vector2 ScreenLocationJoystick;
    public Vector2 TouchJoystickLocation { get { return ScreenLocationJoystick; } }

    private Vector2 WorldLocation;
    public Vector2 TouchWorldLocation { get { return WorldLocation; } }
        // Controls
    [SerializeField]
    private JoyscriptControl Controller;
        // Mouse Fire
    private bool MouseFireStartedOnRight = false;
    private List<FingerAndStart> TouchFireStartedOnRight = new List<FingerAndStart>();
        // A way to keep if the finger started on the right or not
    public struct FingerAndStart
    {
        public Touch FingerTouch;
        public bool StartedRight;

        public void Set(Touch id, bool startedRight)
        {
            FingerTouch = id;
            StartedRight = startedRight;
        }

        public bool Contains(Touch touch)
        {
            return touch.fingerId == FingerTouch.fingerId;
        }
    }

    // Ship Mobility
    [SerializeField]
    private float AccelerationFactor = 10;
    [SerializeField]
    private float DeaccelerationRate = 5;
    [SerializeField]
    private float TopSpeedFactor = 1;
    private int BaseSpeed = 0;
    private int BaseAcceleration= 0;
    private int BaseThrustRight = 0;
    private int BaseThrustLeft = 0;
    private int BaseThrustUp = 0;
    private int BaseThrustDown = 0;
    private int BaseAccelReduction = 0;
    private float CurrentSpeedHorizontal = 0;
    private float CurrentAccelerationHorizontal = 0;
    private float CurrentSpeedVertical = 0;
    private float CurrentAccelerationVertical = 0;
    private float MaxSpeedRight = 0;
    private float MaxSpeedLeft = 0;
    private float MaxSpeedTop = 0;
    private float MaxSpeedBottom = 0;
    // Whether the device is in landscape or potrait mode;
    private float OrientationSpeedMultiplier = 1;
    public float DeviceOrientationSpeedMultiplier
    {
        get
        {
            return OrientationSpeedMultiplier;
        }
        set
        {
            OrientationSpeedMultiplier = value;
            UpdateSpeedLimits();
        }
    }
    public Vector3 GetCurrentVelocity
    {
        get
        {
            return new Vector3(CurrentSpeedHorizontal, CurrentSpeedVertical, 0);
        }
    }
    public Vector3 GetCurrentAcceleration
    {
        get
        {
            return new Vector3(CurrentAccelerationHorizontal, CurrentAccelerationVertical, 0);
        }
    }
    [SerializeField]
    private bool InstantlyChangeDirection = false;
    public bool ChangeDirectionInstantly
    {
        get
        {
            return InstantlyChangeDirection;
        }
    }
    [SerializeField]
    private bool InstantlyGetTopSpeed = false;
    public bool ShipInstantSpeed
    {
        get
        {
            return InstantlyGetTopSpeed;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        Mytransform = GetComponent<Transform>();
        Parts = GetComponentsInChildren<ShipParts>();
        // Set all the part lists
        SetVariables();

        // Set Find Joystick Elements
        SceneEventSystem = FindObjectOfType<EventSystem>();

        // Set Limits
        MaxSpeedRight = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedLeft = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedTop = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustUp + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedBottom = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustDown + BaseAcceleration + 1) / (BaseAccelReduction + 1);
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
        ApplyMobility(Controller.GetMovementRatio());
    }

    // Mobility
    private void SetWingListStats()
    {
        foreach (Wing wing in WingParts)
        {
            BaseSpeed += wing.ShipSpeedIncrease;
            BaseAcceleration += wing.ShipAgilityIncrease;
        }
    }
    private void SetHullListStats()
    {
        foreach (Hull hull in HullParts)
        {
            BaseAccelReduction += hull.ShipSpeedReduction;
        }
    }
    private void SetGunListStats()
    {
        foreach (Weapon weap in WeaponParts)
        {
            if (weap.transform.rotation.z == 0f)
            {
                BaseThrustLeft += weap.PartThrust;
            }
            else if (weap.transform.rotation.z == 90f)
            {
                BaseThrustDown += weap.PartThrust;
            }
            else if (weap.transform.rotation.z == 180f)
            {
                BaseThrustRight += weap.PartThrust;
            }
            else if (weap.transform.rotation.z == 270f)
            {
                BaseThrustUp += weap.PartThrust;
            }
        }
    }
    private void SetThrusterListStats()
    {
        foreach (Thruster thruster in ThrusterParts)
        {
            if (thruster.transform.rotation.z == 0f)
            {
                BaseThrustRight += thruster.PartThrust;
            }
            else if (thruster.transform.rotation.z == 90f)
            {
                BaseThrustUp += thruster.PartThrust;
            }
            else if (thruster.transform.rotation.z == 180f)
            {
                BaseThrustLeft += thruster.PartThrust;
            }
            else if (thruster.transform.rotation.z == 270f)
            {
                BaseThrustDown += thruster.PartThrust;
            }
        }
    }
    private void ApplyMobility(Vector2 DirectionRatio)
    {
        // Calculate
        // Horizontal Movement
        if (DirectionRatio.x > 0) // Right
        {
            //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Right");
            if (InstantlyChangeDirection)
            {
                if (CurrentSpeedHorizontal < 0)
                {
                    CurrentSpeedHorizontal = 0;
                }
            }
            if (ShipInstantSpeed)
            {
                CurrentSpeedHorizontal = TopSpeedFactor * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) * DirectionRatio.x / (BaseAccelReduction + 1);
            }
            else if (CurrentSpeedHorizontal <= MaxSpeedRight)
                CurrentAccelerationHorizontal = DirectionRatio.x * Time.fixedDeltaTime * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) * AccelerationFactor / (BaseAccelReduction + 1);
            else
                CurrentAccelerationHorizontal = 0;
        }
        else if (DirectionRatio.x < 0) // Left
        {
            //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Left");
            if (InstantlyChangeDirection)
            {
                if (CurrentSpeedHorizontal > 0)
                {
                    CurrentSpeedHorizontal = 0;
                }
            }
            if (ShipInstantSpeed)
            {
                CurrentSpeedHorizontal = TopSpeedFactor * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) * DirectionRatio.x / (BaseAccelReduction + 1);
            }
            else if (CurrentSpeedHorizontal <= MaxSpeedLeft)
                CurrentAccelerationHorizontal = DirectionRatio.x * Time.fixedDeltaTime * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) * AccelerationFactor / (BaseAccelReduction + 1);
            else
                CurrentAccelerationHorizontal = 0;
        }
        else
        {
            if (InstantlyChangeDirection)
            {
                CurrentSpeedHorizontal = 0;
            }
            else if (Mathf.Abs(CurrentAccelerationHorizontal) > 0)
                CurrentAccelerationHorizontal = -DeaccelerationRate * Time.fixedDeltaTime * CurrentSpeedHorizontal * (BaseAcceleration + 1) / (BaseAccelReduction + 1);
            else
                CurrentAccelerationHorizontal = 0;
        }
        // Vertical Movement
        if (DirectionRatio.y > 0) // Up
        {
            //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Up");
            if (InstantlyChangeDirection)
            {
                if (CurrentSpeedVertical < 0)
                {
                    CurrentSpeedVertical = 0;
                }
            }
            if (ShipInstantSpeed)
            {
                CurrentSpeedVertical = TopSpeedFactor * BaseSpeed * (BaseThrustUp + BaseAcceleration + 1) * DirectionRatio.y / (BaseAccelReduction + 1);
            }
            else if (CurrentSpeedVertical <= MaxSpeedTop)
                CurrentAccelerationVertical = DirectionRatio.y * Time.fixedDeltaTime * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) * AccelerationFactor / (BaseAccelReduction + 1);
            else
                CurrentAccelerationVertical = 0;
        }
        else if (DirectionRatio.y < 0) // Down
        {
            //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Down");
            if (InstantlyChangeDirection)
            {
                if (CurrentSpeedVertical > 0)
                {
                    CurrentSpeedVertical = 0;
                }
            }
            if (ShipInstantSpeed)
            {
                CurrentSpeedVertical = TopSpeedFactor * BaseSpeed * (BaseThrustDown + BaseAcceleration + 1) * DirectionRatio.y / (BaseAccelReduction + 1);
            }
            else if (CurrentSpeedVertical <= MaxSpeedBottom)
                CurrentAccelerationVertical = DirectionRatio.y * Time.fixedDeltaTime * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) * AccelerationFactor / (BaseAccelReduction + 1);
            else
                CurrentAccelerationVertical = 0;
        }
        else
        {
            if (InstantlyChangeDirection)
            {
                CurrentSpeedVertical = 0;
            }
            else if (Mathf.Abs(CurrentAccelerationVertical) > 0)
                CurrentAccelerationVertical = -DeaccelerationRate * Time.fixedDeltaTime * CurrentSpeedVertical * (BaseAcceleration + 1) / (BaseAccelReduction + 1);
            else
                CurrentAccelerationVertical = 0;
        }
        //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ratio: " + DirectionRatio);

        // Adjust Speed
        CurrentSpeedHorizontal += CurrentAccelerationHorizontal * Time.fixedDeltaTime;
        // Make sure the ship is not speeding
        if (CurrentSpeedHorizontal > TopSpeedFactor * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) / (BaseAccelReduction + 1))
            CurrentSpeedHorizontal = TopSpeedFactor * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        else if (CurrentSpeedHorizontal < -TopSpeedFactor * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) / (BaseAccelReduction + 1))
            CurrentSpeedHorizontal = -TopSpeedFactor * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) / (BaseAccelReduction + 1);

        CurrentSpeedVertical += CurrentAccelerationVertical * Time.fixedDeltaTime;
        // Make sure the ship is not speeding
        if (CurrentSpeedVertical > TopSpeedFactor * BaseSpeed * (BaseThrustUp + BaseAcceleration + 1) / (BaseAccelReduction + 1))
            CurrentSpeedVertical = TopSpeedFactor * BaseSpeed * (BaseThrustUp + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        else if (CurrentSpeedVertical < -TopSpeedFactor * BaseSpeed * (BaseThrustDown + BaseAcceleration + 1) / (BaseAccelReduction + 1))
            CurrentSpeedVertical = -TopSpeedFactor * BaseSpeed * (BaseThrustDown + BaseAcceleration + 1) / (BaseAccelReduction + 1);

        Vector3 Speed = new Vector3(CurrentSpeedHorizontal, CurrentSpeedVertical, 0);

        // Apply Speeds
        //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Velocity: " + Speed);
        Mytransform.position += Speed;
    }

    private void UpdateSpeedLimits()
    {
        MaxSpeedRight = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustRight + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedLeft = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustLeft + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedTop = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustUp + BaseAcceleration + 1) / (BaseAccelReduction + 1);
        MaxSpeedBottom = OrientationSpeedMultiplier * TopSpeedFactor * BaseSpeed * (BaseThrustDown + BaseAcceleration + 1) / (BaseAccelReduction + 1);
    }

    // Weapons
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

    // Setting list of parts
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
        // Set mobility variables
        SetWingListStats();
        SetHullListStats();
        SetGunListStats();
        SetThrusterListStats();
    }

    // Touch and Control
    private bool ContainsTouch(List<FingerAndStart> StructList, Touch touch)
    {
        bool Result = false;
        foreach (FingerAndStart TouchStruct in StructList)
        {
            if (TouchStruct.Contains(touch))
            {
                Result = true;
                break;
            }
        }

        return Result;
    }

    // Make sure to check you got a valid Struct
    private FingerAndStart GetTouchStruct(List<FingerAndStart> StructList, Touch touch)
    {
        FingerAndStart Result = new FingerAndStart();
        foreach (FingerAndStart TouchStruct in StructList)
        {
            if (TouchStruct.Contains(touch))
            {
                Result = TouchStruct;
                break;
            }
        }

        return Result;
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            NoTouchesRight = true;
            foreach (Touch touch in Input.touches)
            {
                DecideTouchDecision(touch);
            }
            if (NoTouchesRight)
            {
                if (NoTouchWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.NoTouch);
            }
        }
    }

    private void DecideTouchDecision(Touch touch)
    {
        ScreenLocation = touch.position;
        //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Position: " + ScreenLocation);
        Ray ray = Camera.main.ScreenPointToRay(ScreenLocation);
        MouseFireStartedOnRight = false;
        // Make sure its part of a valid struct
        if (ContainsTouch(TouchFireStartedOnRight, touch))
        {
            MouseFireStartedOnRight = GetTouchStruct(TouchFireStartedOnRight, touch).StartedRight;
        }


        if (ScreenLocation.x >= Screen.width / 2 || MouseFireStartedOnRight)
        {
            NoTouchesRight = false;
            // OnDown
            if (touch.phase == TouchPhase.Began)
            {
                if (!ContainsTouch(TouchFireStartedOnRight, touch))
                {
                    FingerAndStart temp = new FingerAndStart();
                    temp.Set(touch, true);
                    TouchFireStartedOnRight.Add(temp);
                }
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Began");
                if (OnDownWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnDown);
            }
            // Hold
            else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Hold");
                if (HoldWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.Hold);
            }
            // On Up
            else if (touch.phase == TouchPhase.Ended)
            {
                // remove from list if it contains it
                if (ContainsTouch(TouchFireStartedOnRight, touch))
                {
                    TouchFireStartedOnRight.Remove(GetTouchStruct(TouchFireStartedOnRight, touch));
                }
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ended");
                if (OnUpWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnUp);
            }
        }
        else
        {
            if (touch.phase == TouchPhase.Began)
            {
                FingerAndStart temp = new FingerAndStart();
                temp.Set(touch, false);
                TouchFireStartedOnRight.Add(temp);
            }
        }

        // Update joystick touch parameters
        if (touch.fingerId == FingerIdFollow)
        {
            ScreenLocationJoystick = touch.position;
        }

        // If player clicks on the joystick
        if (touch.phase == TouchPhase.Began && !Controller.IsPressed && ScreenLocation.x < Screen.width / 2)
        {
            FingerIdFollow = touch.fingerId;
            ScreenLocationJoystick = touch.position;
            // Get Event data
            JoystickEventData = new PointerEventData(SceneEventSystem);
            JoystickEventData.position = new Vector3(ScreenLocation.x, ScreenLocation.y, 0);
            List<RaycastResult> RaycastResults = new List<RaycastResult>();
            JoystickHitRaycastor.Raycast(JoystickEventData, RaycastResults);

            //If we found the joystick
            if (UiFound(RaycastResults))
            {
                Controller.StartFollow(this);
            }
        }
        else if (touch.phase == TouchPhase.Ended && touch.fingerId == FingerIdFollow)
        {
            FingerIdFollow = -1;
            // remove from list if it contains it
            if (ContainsTouch(TouchFireStartedOnRight, touch))
            {
                TouchFireStartedOnRight.Remove(GetTouchStruct(TouchFireStartedOnRight, touch));
            }

            // Reset this just ion case
            MouseFireStartedOnRight = false;

            // Let the controller no it can stop following
            Controller.IsPressed = false;
        }
    }
    private void MouseInput()
    {
        // Find mouse position
        ScreenLocation = Input.mousePosition;
        ScreenLocationJoystick = ScreenLocation;

        if (ScreenLocation.x >= Screen.width / 2.0f || MouseFireStartedOnRight)
        {
            // Right Mouse acts like a touch 
            // OnDown
            if (Input.GetMouseButtonDown(1))
            {
                MouseFireStartedOnRight = true;
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Began");
                if (OnDownWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnDown);
            }
            // Hold
            else if (Input.GetMouseButton(1) && MouseFireStartedOnRight)
            {
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Hold");
                if (HoldWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.Hold);
            }
            // On Up
            else if (Input.GetMouseButtonUp(1) && MouseFireStartedOnRight)
            {
                MouseFireStartedOnRight = false;
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ended");
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
        // Adds another touch to test fireing and moving
        else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Began");
                if (OnDownWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnDown);
            }
            // Hold
            else if (Input.GetMouseButton(0))
            {
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Hold");
                if (HoldWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.Hold);
            }
            // On Up
            else if (Input.GetMouseButtonUp(0))
            {
                //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Ended");
                if (OnUpWeapons.Count > 0)
                    FireWeapons(Weapon.TypeOfFire.OnUp);
            }
        }
        else
        {
            // No touching the fire so ship fires no touch weapons
            if (NoTouchWeapons.Count > 0)
                FireWeapons(Weapon.TypeOfFire.NoTouch);
        }

        // If player clicks on the joystick
        if (Input.GetMouseButtonDown(1))
        {
            // Get Event data
            JoystickEventData = new PointerEventData(SceneEventSystem);
            JoystickEventData.position = new Vector3(ScreenLocation.x, ScreenLocation.y, 0);
            List<RaycastResult> RaycastResults = new List<RaycastResult>();
            JoystickHitRaycastor.Raycast(JoystickEventData, RaycastResults);

            //If we found the joystick
            if (UiFound(RaycastResults))
            {
                Controller.StartFollow(this);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            // Reset this just ion case
            MouseFireStartedOnRight = false;

            // Let the controller no it can stop following
            Controller.IsPressed = false;
        }
    }

    private bool UiFound(List<RaycastResult> results)
    {
        bool found = false;
        foreach (RaycastResult result in results)
        {
            if(result.gameObject.tag == "LeftJoystick")
            {
                found = true;
                break;
            }
        }
        return found;
    }
}