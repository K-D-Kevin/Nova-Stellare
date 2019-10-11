using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderSlot : MonoBehaviour
{
    // Corresponding Ship part
    private ShipParts SlotPart;

    public ShipParts BuilderPart
    {
        get
        {
            return SlotPart;
        }
        set
        {
            SlotPart = value;
        }
    }

    // See if the part can be put into the slot
    [SerializeField]
    private RawImage ColorChangeImage;
    [SerializeField]
    private RawImage PartImage;
    [SerializeField]
    private Color Open_Color;
    [SerializeField]
    private Color Closed_Color;
}
