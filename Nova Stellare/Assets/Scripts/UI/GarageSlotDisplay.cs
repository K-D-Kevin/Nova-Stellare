using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageSlotDisplay : MonoBehaviour
{
    /// <summary>
    /// Class to keep track of garage slots and display
    /// </summary>

    [SerializeField]
    private GarageSlot GaragePrefab;
    [SerializeField]
    private List<GarageSlot> GarageSlots = new List<GarageSlot>();
    public List<GarageSlot> GetGarageSlots
    {
        get
        {
            return GarageSlots;
        }
    }
    [SerializeField]
    private GameObject NextPage;
    public GameObject GetNextPage
    {
        get
        {
            return NextPage;
        }
    }
    [SerializeField]
    private MovieImage LoadingScreen;
    [SerializeField]
    private SelectedShipPage SelectPage;
    private RectTransform MyTransform;
    private int AvailibleSlots = 0;
    private int FilledSlots = 0;

    private void Start()
    {
        MyTransform = GetComponent<RectTransform>();
        Redraw();
    }

    public void AddGarageSlot()
    {
        if (GaragePrefab != null)
        {
            GarageSlot Temp = Instantiate(GaragePrefab, MyTransform);
            Temp.GarageParent = this;
            Temp.Initialize(NextPage);
            GarageSlots.Add(Temp);
            Redraw();
        }
    }

    // Resort the garage slots
    public void Redraw()
    {
        for (int i = 0; i < GarageSlots.Count; i++)
        {
            GarageSlots[i].Start();
            //GarageSlots[GarageSlots.Count - i - 1].GetTransform.localPosition = new Vector3((GarageSlots.Count - i - 1) * (GarageSlots[GarageSlots.Count - i - 1].GetTransform.rect.width + 50) + GarageSlots[GarageSlots.Count - i - 1].GetTransform.rect.width / 2 + 25, -GarageSlots[GarageSlots.Count - i - 1].GetTransform.rect.height / 2 - 100, 0);
        }
    }

    public void OnClick()
    {
        AddGarageSlot();
    }

    public void MoveLoadingScreen()
    {
        LoadingScreen.Move();
    }

    public void SetGarageSlot(GarageSlot slot)
    {
        SelectPage.SelectedShip = slot;
        SelectPage.UpdateScreen();
    }
}
