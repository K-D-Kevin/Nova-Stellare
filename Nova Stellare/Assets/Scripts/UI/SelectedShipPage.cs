using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedShipPage : MonoBehaviour
{
    // Ship Infomation
    private GarageSlot ShipGarageSlot;
    public GarageSlot SelectedShip
    {
        get
        {
            return ShipGarageSlot;
        }
        set
        {
            ShipGarageSlot = value;
        }
    }

    // Page Components
    [SerializeField]
    private TextDisplay InspectorText;
    [SerializeField]
    private TextDisplay BuilderText;
    [SerializeField]
    private TextDisplay UpgradeText;
    [SerializeField]
    private TextDisplay DestroyText;

    public void UpdateScreen()
    {
        // Determine which buttons are supposed to be visible respective to each ship
        if (ShipGarageSlot)
        {
            if (ShipGarageSlot.gameObject.name == "Default" || ShipGarageSlot.gameObject.name == "Empty")
            {
                DestroyText.gameObject.SetActive(false);
                if (ShipGarageSlot.gameObject.name == "Default")
                {
                    BuilderText.gameObject.SetActive(false);
                }
                if (ShipGarageSlot.gameObject.name == "Empty")
                {
                    InspectorText.gameObject.SetActive(false);
                }
            }
            else
            {
                DestroyText.gameObject.SetActive(true);
                InspectorText.gameObject.SetActive(true);
                BuilderText.gameObject.SetActive(true);
            }
        }
    }

    public void ResetAll()
    {
        DestroyText.gameObject.SetActive(true);
        InspectorText.gameObject.SetActive(true);
        BuilderText.gameObject.SetActive(true);
        UpgradeText.gameObject.SetActive(true);
    }
}
