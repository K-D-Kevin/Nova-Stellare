using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSoftHardHitboxes : MonoBehaviour
{
    /// <summary>
    /// The class is to fit box colliders around camera view
    /// </summary>

    // The colliders
    [SerializeField]
    private BoxCollider2D LeftSoft;
    [SerializeField]
    private BoxCollider2D RightSoft;
    [SerializeField]
    private BoxCollider2D TopSoft;
    [SerializeField]
    private BoxCollider2D BottomSoft;
    [SerializeField]
    private BoxCollider2D LeftHard;
    [SerializeField]
    private BoxCollider2D RightHard;
    [SerializeField]
    private BoxCollider2D TopHard;
    [SerializeField]
    private BoxCollider2D BottomHard;

    // Set Hitbox sizes
    [SerializeField]
    private Vector2 HardSoftLeftRanges; // x = hard / y = soft ranges
    [SerializeField]
    private Vector2 HardSoftRightRanges; // x = hard / y = soft ranges
    [SerializeField]
    private Vector2 HardSoftTopRanges; // x = hard / y = soft ranges
    [SerializeField]
    private Vector2 HardSoftBottomRanges; // x = hard / y = soft ranges

    // Screen sizes
    private float ScreenWidth;
    private float ScreenHeight;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBoxes();
    }

    public void UpdateBoxes()
    {
        mainCam = Camera.main;
        Vector3 pos = new Vector3(Screen.width, Screen.height, -100);
        Vector3 TopRight = mainCam.ScreenToWorldPoint(pos);
        Vector3 BottomLeft = mainCam.ScreenToWorldPoint(-pos);
        ScreenWidth = TopRight.x - BottomLeft.x;
        ScreenHeight = TopRight.y - BottomLeft.y;


        // Adjust Colliders
        // Left
        LeftSoft.size = new Vector2(HardSoftLeftRanges.x, ScreenHeight);
        LeftHard.size = new Vector2(HardSoftLeftRanges.y, ScreenHeight);

        // Right
        RightSoft.size = new Vector2(HardSoftRightRanges.x, ScreenHeight);
        RightHard.size = new Vector2(HardSoftRightRanges.y, ScreenHeight);

        // Top
        TopSoft.size = new Vector2(ScreenWidth, HardSoftTopRanges.x);
        TopHard.size = new Vector2(ScreenWidth, HardSoftTopRanges.y);

        // Bottom
        BottomSoft.size = new Vector2(ScreenWidth, HardSoftBottomRanges.x);
        BottomHard.size = new Vector2(ScreenWidth, HardSoftBottomRanges.y);
    }
}
