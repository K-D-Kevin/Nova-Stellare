using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrientationAdjustor : MonoBehaviour
{
    [SerializeField]
    private Transform RotationPoint;
    [SerializeField]
    private List<Transform> RotateTransform = new List<Transform>();
    [SerializeField]
    private List<GameObject> LandscapeObjects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> PotraitObjects = new List<GameObject>();

    [SerializeField]
    private RectTransform CanvasTransform;
    [SerializeField]
    private float PotraitRelativeSclae = 1;
    private float LandScapeScale = -1;
    private float PotraitScale = -1;
    private DeviceOrientation LastOrientation = DeviceOrientation.Unknown;
    private DeviceOrientation CurrentOrientation = DeviceOrientation.Unknown;
    public DeviceOrientation CurrentDeviceOrientation
    {
        get
        {
            return CurrentOrientation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft && LastOrientation != DeviceOrientation.LandscapeLeft)
        {
            CurrentOrientation = DeviceOrientation.LandscapeLeft;
            if (LandScapeScale == -1)
            {
                LandScapeScale = CanvasTransform.localScale.x;
            }
            if (LastOrientation == DeviceOrientation.Portrait || LastOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                RotateTransforms(-90);
            }
            LastOrientation = DeviceOrientation.LandscapeLeft;
        }
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight && LastOrientation != DeviceOrientation.LandscapeRight)
        {
            CurrentOrientation = DeviceOrientation.LandscapeRight;
            if (LandScapeScale == -1)
            {
                LandScapeScale = CanvasTransform.localScale.x;
            }
            if (LastOrientation == DeviceOrientation.Portrait || LastOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                RotateTransforms(-90);
            }
            LastOrientation = DeviceOrientation.LandscapeRight;
        }
        else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown && LastOrientation != DeviceOrientation.PortraitUpsideDown)
        {
            CurrentOrientation = DeviceOrientation.PortraitUpsideDown;
            if (PotraitScale == -1)
            {
                PotraitScale = CanvasTransform.localScale.x;
            }
            if (LastOrientation == DeviceOrientation.LandscapeRight || LastOrientation == DeviceOrientation.LandscapeLeft)
            {
                RotateTransforms(90);
            }
            LastOrientation = DeviceOrientation.PortraitUpsideDown;
        }
        else if (Input.deviceOrientation == DeviceOrientation.Portrait && LastOrientation != DeviceOrientation.Portrait)
        {
            CurrentOrientation = DeviceOrientation.Portrait;
            if (PotraitScale == -1)
            {
                PotraitScale = CanvasTransform.localScale.x;
            }
            if (LastOrientation == DeviceOrientation.LandscapeRight || LastOrientation == DeviceOrientation.LandscapeLeft)
            {
                RotateTransforms(90);
            }
            LastOrientation = DeviceOrientation.Portrait;
        }
    }

    public void RotateTransforms(float angle = 0)
    {
        // If Orientation Fundamentally changed either from potrait to landscape or landscape to potrait
        bool ChangedPotraitToLandscape = (LastOrientation == DeviceOrientation.Portrait || LastOrientation == DeviceOrientation.PortraitUpsideDown) && (CurrentOrientation == DeviceOrientation.LandscapeRight || CurrentOrientation == DeviceOrientation.LandscapeLeft);
        bool ChangedLandscapeToPotrait = (CurrentOrientation == DeviceOrientation.Portrait || CurrentOrientation == DeviceOrientation.PortraitUpsideDown) && (LastOrientation == DeviceOrientation.LandscapeRight || LastOrientation == DeviceOrientation.LandscapeLeft);

        if (ChangedLandscapeToPotrait || ChangedPotraitToLandscape)
        {
            foreach (GameObject G in PotraitObjects)
            {
                if (ChangedLandscapeToPotrait)
                {
                    G.SetActive(true);
                }
                else if (ChangedPotraitToLandscape)
                {
                    G.SetActive(false);
                }
            }

            foreach (GameObject G in LandscapeObjects)
            {
                if (ChangedPotraitToLandscape)
                {
                    G.SetActive(true);
                }
                else if (ChangedLandscapeToPotrait)
                {
                    G.SetActive(false);
                }
            }
        }

        FindObjectOfType<InBuildDebugger>().SendDebugMessege("LS/PS: " + LandScapeScale + "/" + PotraitScale);
        // Each set object set parent to be rotated
        RotationPoint.localRotation = Quaternion.identity;
        List<Transform> OldParents = new List<Transform>();
        foreach (Transform T in RotateTransform)
        {
            OldParents.Add(T.parent);
            if (ChangedLandscapeToPotrait)
            {
                T.localScale *= PotraitScale * PotraitRelativeSclae / LandScapeScale;
            }
            else if (ChangedPotraitToLandscape)
            {
                T.localScale *= LandScapeScale / (PotraitScale * PotraitRelativeSclae);
            }
            T.SetParent(RotationPoint);
        }
        if (ChangedLandscapeToPotrait)
        {
            FindObjectOfType<PlayerShip>().DeviceOrientationSpeedMultiplier = PotraitScale * PotraitRelativeSclae / LandScapeScale;
        }
        else if (ChangedPotraitToLandscape)
        {
            FindObjectOfType<PlayerShip>().DeviceOrientationSpeedMultiplier = LandScapeScale / (PotraitScale * PotraitRelativeSclae);
        }

        // Rotate objects
        RotationPoint.Rotate(0, 0, angle);

        // Unparent Objects
        for(int i = 0; i < RotateTransform.Count; i++)
        {
            RotateTransform[i].SetParent(OldParents[i]);
        }
    }
}
