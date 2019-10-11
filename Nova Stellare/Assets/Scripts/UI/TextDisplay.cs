using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    // Size Variables
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

    [SerializeField]
    private float Center;
    public float GetCenter
    {
        get
        {
            return Center;
        }
    }

    // Where to center the text
    public enum RelativeCenter
    {
        Left = 0,
        Middle = 1,
        Right = 2,
    }
    [SerializeField]
    private RelativeCenter CenterAt;
    public RelativeCenter GetCenterAt
    {
        get
        {
            return CenterAt;
        }
    }

    // Get Components
    // Text
    [SerializeField]
    private Text DisplayText;
    public Text GetDisplayText
    {
        get
        {
            return DisplayText;
        }
    }

    [SerializeField]
    private string DisplayStr;
    public string GetDisplayStr
    {
        get
        {
            return DisplayStr;
        }
    }
    // Button
    [SerializeField]
    private Button DisplayButton;
    public Button GetDisplayButton
    {
        get
        {
            return DisplayButton;
        }
    }

    public bool DisplayButtonActive = true;
    public bool DisplayButtonInteractable = true;

    // Resize Variables
    RectTransform[] Children;
    RectTransform EdgeChild;
    RectTransform BackgroundChild;
    RectTransform ButtonChild;
    RectTransform TextChild;
    bool SetChildren = false;

    public void SetChildTransforms()
    {
        Children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform RT in Children)
        {
            if (RT.name == "Edge")
                EdgeChild = RT;
            else if (RT.name == "Background")
                BackgroundChild = RT;
            else if (RT.name == "Text")
                TextChild = RT;
            else if (RT.name == "Button")
                ButtonChild = RT;
        }
        SetChildren = true;
    }

    //[ExecuteAlways]
    public void SetDisplay(string text, Vector2 EdgeSize, Vector2 BufferSize, bool isInteractable = true, bool isEnabled = true)
    {
        DisplayStr = text;
        EdgePixelSize = EdgeSize;
        TitleBufferSize = BufferSize;
        DisplayButtonInteractable = isInteractable;
        DisplayButtonActive = isEnabled;
        //Debug.Log(text);
    }

    public void SetDisplay(string text, float EdgeSize = 5, float BufferSize = 0, bool isInteractable = true, bool isEnabled = true)
    {
        SetDisplay(text, new Vector2(EdgeSize, EdgeSize), new Vector2(BufferSize, BufferSize), isInteractable, isEnabled);
    }

    public void SetDisplay(string text, float EdgeSize, Vector2 BufferSize, bool isInteractable = true, bool isEnabled = true)
    {
        SetDisplay(text, new Vector2(EdgeSize, EdgeSize), BufferSize, isInteractable, isEnabled);
    }

    public void SetDisplay(string text, Vector2 EdgeSize, float BufferSize = 0, bool isInteractable = true, bool isEnabled = true)
    {
        SetDisplay(text, EdgeSize, new Vector2(BufferSize, BufferSize), isInteractable, isEnabled);
    }

    public void Start()
    {
        DisplayText.text = DisplayStr;
    }

    public void UpdateTextDisplay()
    {
        DisplayText.text = DisplayStr;
        Resize();
    }
    public void Resize()
    {
        // Need Children to set sizes
        if (!SetChildren)
            SetChildTransforms();

        float prefferedH = DisplayText.preferredHeight;
        float prefferedW = DisplayText.preferredWidth;
        Vector2 BaseObjectResize = new Vector2(prefferedW, prefferedH) + TitleBufferSize;
        Vector2 EdgeObjectResize = BaseObjectResize + EdgePixelSize;

        GetComponent<RectTransform>().sizeDelta = EdgeObjectResize;
        TextChild.sizeDelta = BaseObjectResize;
        BackgroundChild.sizeDelta = BaseObjectResize;
        EdgeChild.sizeDelta = EdgeObjectResize;
        ButtonChild.sizeDelta = EdgeObjectResize;
        float AmountToMove = 0;
        if (CenterAt == TextDisplay.RelativeCenter.Left)
            AmountToMove = EdgeObjectResize.x / 2 + Center;
        else if (CenterAt == TextDisplay.RelativeCenter.Middle)
            AmountToMove = Center;
        else if (CenterAt == TextDisplay.RelativeCenter.Right)
            AmountToMove = Center - EdgeObjectResize.x / 2;
        RectTransform RT = GetComponent<RectTransform>();
        try
        {
            RectTransform CS_RT = GetComponentInParent<CanvasScaler>().GetComponent<RectTransform>();
            //Debug.Log("Amount to Move: " + AmountToMove + ", Scale Factor: " + CS_RT.localScale.x + ", Amount to Move*: " + AmountToMove * CS_RT.localScale.x);
            RT.position = new Vector3(AmountToMove * CS_RT.localScale.x, transform.position.y, transform.position.z);
        }
        catch
        {
            RT.position = new Vector3(AmountToMove, transform.position.y, transform.position.z);
        }
    }
}
