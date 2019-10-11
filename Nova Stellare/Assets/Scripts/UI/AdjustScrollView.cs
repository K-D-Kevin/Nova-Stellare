using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustScrollView : MonoBehaviour
{
    // Components
    private RectTransform MyTransform;
    [SerializeField]
    private RectTransform LeftCorner;
    [SerializeField]
    private RectTransform RightCorner;
    [SerializeField]
    private RectTransform LeftStopper;
    [SerializeField]
    private RectTransform RightStopper;

    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftCorner.localPosition.x > LeftStopper.localPosition.x)
        {
            MyTransform.localPosition = LeftStopper.localPosition;
        }
        else if (RightCorner.localPosition.x > LeftStopper.localPosition.x)
        {
            MyTransform.localPosition = RightStopper.localPosition;
        }
    }
}
