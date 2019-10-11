using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
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
    private Vector2 Center;
    public Vector2 GetCenter
    {
        get
        {
            return Center;
        }
    }

    // Where to center the text
    public enum ImageCenter
    {
        // Middle Row
        LeftTop = 0,
        LeftMiddle = 1,
        LeftBottom = 2,
        CenterTop = 3,
        CenterMid = 4,
        CenterBottom = 5,
        RightTop = 6,
        RightMiddle = 7,
        RightBottom = 8,
    }
    [SerializeField]
    private ImageCenter CenterAt;
    public ImageCenter GetCenterAt
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
    // Image 
    [SerializeField]
    private RawImage DisplayImage;
    public RawImage GetDisplayImage
    {
        get
        {
            return DisplayImage;
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
    public bool UseCenter = true;

    //[ExecuteAlways]
    public void SetDisplay(string text, Vector2 EdgeSize, Vector2 BufferSize, RawImage MainImage = null,bool isInteractable = true, bool isEnabled = true)
    {
        DisplayStr = text;
        EdgePixelSize = EdgeSize;
        TitleBufferSize = BufferSize;
        DisplayButtonInteractable = isInteractable;
        DisplayButtonActive = isEnabled;
        if (MainImage != null)
            DisplayImage = MainImage;
    }
    public void SetDisplay(string text, float EdgeSize, float BufferSize, RawImage MainImage = null, bool isInteractable = true, bool isEnabled = true)
    {
        SetDisplay(text, new Vector2(EdgeSize, EdgeSize), new Vector2(BufferSize, BufferSize), MainImage, isInteractable, isEnabled);
    }

    public void Start()
    {
        DisplayText.text = DisplayStr;
    }
}
