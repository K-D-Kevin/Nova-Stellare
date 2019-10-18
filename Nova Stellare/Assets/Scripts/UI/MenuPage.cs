using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    // Other Objects
    [SerializeField]
    private GameObject ParentObject;
    public GameObject GetParent
    {
        get
        {
            return ParentObject;
        }
    }
    [SerializeField]
    private RectTransform FollowingPagesTransform;
    public RectTransform GetFollowingPages
    {
        get
        {
            return FollowingPagesTransform;
        }
    }
    [SerializeField]
    private MenuPage DefualtPagePrefab;
    public MenuPage GetDefualtPagePrefab
    {
        get
        {
            return DefualtPagePrefab;
        }
    }
    [SerializeField]
    private TextDisplay DefualtTextDisplayPrefab;
    public TextDisplay GetDefualtTextDisplayPrefab
    {
        get
        {
            return DefualtTextDisplayPrefab;
        }
    }

    // Variables
    public bool IsTitle = false;
    [SerializeField]
    private TextDisplay TitleTextDisplay;
    public TextDisplay GetTitleTextDisplay
    {
        get
        {
            return GetTitleTextDisplay;
        }
    }
    [SerializeField]
    private TextDisplay ExitTextDisplay;
    public TextDisplay GetExitTextDisplay
    {
        get
        {
            return ExitTextDisplay;
        }
    }

    [SerializeField]
    private string PageName;
    public string GetPageName
    {
        get
        {
            return PageName;
        }
    }

    // If Title Page
    [SerializeField]
    private int MaxPages = 1;
    public int GetMaxPages
    {
        get
        {
            return MaxPages;
        }
    }
    public string NewPageName;
    [SerializeField]
    private int CenteredOnPixel = 100; // X coordinate
    public int GetCenteredOnPixel
    {
        get
        {
            return CenteredOnPixel;
        }
    }
    // CorrespodingPages
    public List<MenuPage> CorrespondingPages = new List<MenuPage>();

    // Button Sizes
    [SerializeField]
    private Vector2 EdgePixelSize;
    public Vector2 GetEdgeSize
    {
        get
        {
            return EdgePixelSize;
        }
    }

    [SerializeField]
    private Vector2 TitleBufferSize;
    public Vector2 GetTitleBufferSize
    {
        get
        {
            return TitleBufferSize;
        }
    }

    // If not Title Page


    //[ExecuteAlways]
    public void AddPage()
    {
        MenuPage temp = Instantiate(DefualtPagePrefab, FollowingPagesTransform);
        //temp.GetExitTextDisplay.SetDisplay("Title Page", new Vector2(30, 30), new Vector2(100, 10), true, true);
        //temp.GetTitleTextDisplay.SetDisplay("Title Page", new Vector2(30, 30), new Vector2(100, 10), true, false);
        CorrespondingPages.Add(temp);
    }

    public void ClearPages()
    {
        TextDisplay[] TDLIST = GetComponentsInChildren<TextDisplay>();
        List<TextDisplay> TDLIST_Delete = new List<TextDisplay>();
        foreach (TextDisplay TD in TDLIST)
        {
            if (TD.gameObject.name == "Text Display(Clone)")
                TDLIST_Delete.Add(TD);
        }
        for (int i = 0; i < CorrespondingPages.Count; i++)
        {
            DestroyImmediate(CorrespondingPages[i].gameObject);
        }
        for (int i = 0; i < TDLIST_Delete.Count; i++)
        {
            DestroyImmediate(TDLIST_Delete[i].gameObject);
        }
        CorrespondingPages.Clear();
    }

    public void FindCorrespondingPages()
    {
        if (FollowingPagesTransform)
        {
            CorrespondingPages.Clear();
            MenuPage[] Pages = FollowingPagesTransform.GetComponentsInChildren<MenuPage>();
            foreach (MenuPage MP in Pages)
            {
                CorrespondingPages.Add(MP);
            }
        }
    }

    public void AdjustTitle(string title)
    {
        TitleTextDisplay.SetDisplay(title, 10, new Vector2(60, 10));
        TitleTextDisplay.UpdateTextDisplay();
    }
}
