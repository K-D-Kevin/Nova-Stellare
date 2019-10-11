using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageSlot : MonoBehaviour
{
    /// <summary>
    /// Class that stores info on the garage slot
    /// </summary>
    //Components
    private ImageDisplay MyImageDisplay;
    public ImageDisplay GetImageDisplay
    {
        get
        {
            return GetImageDisplay;
        }
    }
    private RectTransform MyTransform;
    public RectTransform GetTransform
    {
        get
        {
            return MyTransform;
        }
    }
    [SerializeField]
    private string ShipName = "Empty";
    public string GetShipName
    {
        get
        {
            return ShipName;
        }
    }

    // Other Objects
    [SerializeField]
    private GarageSlotDisplay Parent;
    public GarageSlotDisplay GarageParent
    {
        get
        {
            return Parent;
        }
        set
        {
            Parent = value;
        }
    }
    // Ship in Garage
    [SerializeField]
    private PlayerShip GarageShip;
    public PlayerShip GetGarageShip
    {
        get
        {
            return GarageShip;
        }
        set
        {
            GarageShip = value;
        }
    }
    [SerializeField]
    private GameObject NextPage;
    
    public void Start()
    {
        MyImageDisplay = GetComponent<ImageDisplay>();
        MyTransform = GetComponent<RectTransform>();
        MyImageDisplay.SetDisplay(ShipName, 5, 5);
        gameObject.name = ShipName;
    }

    public void OnClick()
    {
        if (gameObject.name != "Add Slot")
        {
            if (!Parent)
            {
                Parent = GetComponentInParent<GarageSlotDisplay>();
            }
            NextPage.SetActive(true);
            NextPage.GetComponent<MenuPage>().AdjustTitle(ShipName);
            Parent.SetGarageSlot(this);
            Parent.MoveLoadingScreen();
        }
        else
        {
            if (!Parent)
            {
                Parent = GetComponentInParent<GarageSlotDisplay>();
            }
            Parent.OnClick();
        }
    }

    public void Initialize(GameObject Next_Page)
    {
        NextPage = Next_Page;
    }
}
