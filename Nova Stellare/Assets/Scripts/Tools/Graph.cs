using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    ///
    /// Send Graph a set of variables to show a table - Can be used to help debug, in game or ust make a graph
    ///

    // Components
    [SerializeField]
    private TextDisplay Title;
    [SerializeField]
    private TextDisplay X_Axis;
    [SerializeField]
    private TextDisplay Y_Axis;
    [SerializeField]
    private List<TextDisplay> X_Axis_Values = new List<TextDisplay>();
    [SerializeField]
    private List<TextDisplay> Y_Axis_Values = new List<TextDisplay>();
    [SerializeField]
    private RectTransform NewZeroTransformY; // When Min Value is below zero
    [SerializeField]
    private RectTransform NewZeroTransformX; // When Min Value is below zero
    [SerializeField]
    private RectTransform ZeroZeroTransform; // Origin Point
    [SerializeField]
    private RectTransform TenTenTransform; // Max Point
    [SerializeField]
    private RectTransform DotsParent;
    [SerializeField]
    private GraphDataPoint DotPrefab;
    private List<GraphDataPoint> DotList = new List<GraphDataPoint>();

    // Values
    [SerializeField]
    private Vector2 StartMaxValue;
    public Vector2 MaxValueAtStart
    {
        get
        {
            return StartMaxValue;
        }
    }
    [SerializeField]
    private Vector2 StartZeroValue;
    public Vector2 ZeroValueAtStart
    {
        get
        {
            return StartZeroValue;
        }
    }
    private Vector2 MaxValue;
    public Vector2 GraphMaxValue
    {
        get
        {
            return MaxValue;
        }
    }
    private Vector2 ZeroValue;
    public Vector2 GraphZeroValue
    {
        get
        {
            return ZeroValue;
        }
    }

    // Operation Variables
    private bool ResizedGraph = false;

    private void Awake()
    {
        ZeroValue = StartZeroValue;
        MaxValue = MaxValueAtStart;
    }

    public void AddDataPoint(Vector2 Point)
    {
        // Make a new point
        GraphDataPoint NewDot = Instantiate(DotPrefab, DotsParent);
        NewDot.DataPoint = Point;
        DotList.Add(NewDot);

        // Sort it
        ResizedGraph = true; // New Point, so has to be resized
        // Scales the graph if the max or lowest values need to be replaced
        if (Point.x < ZeroValue.x)
        {
            ZeroValue.x = Point.x;
        }
        else if (Point.x > MaxValue.x)
        {
            MaxValue.x = Point.x;
        }
        // Scales the graphs more if data points are far from max or zero axis
        //if (MaxValue.x != 0)
        //{
        //    if (Mathf.Abs(Point.x / MaxValue.x) < .1f)
        //    {
        //        MaxValue.x = Point.x;
        //    }
        //}
        //if (Point.x != 0)
        //{
        //    if (Mathf.Abs(ZeroValue.x / Point.x) < .1f)
        //    {
        //        ZeroValue.x = Point.x;
        //    }
        //}
        // Scales the graph if the max or lowest values need to be replaced
        if (Point.y < ZeroValue.y)
        {
            ZeroValue.y = Point.y;
        }
        else if (Point.y > MaxValue.y)
        {
            MaxValue.y = Point.y;
        }
        // Scales the graphs more if data points are far from max or zero axis
        //if (MaxValue.y != 0)
        //{
        //    if (Mathf.Abs(Point.y / MaxValue.y) < .1f)
        //    {
        //        MaxValue.y = Point.y;
        //    }
        //}
        //if (Point.y != 0)
        //{
        //    if (Mathf.Abs(ZeroValue.y / Point.y) < .1f)
        //    {
        //        ZeroValue.y = Point.y;
        //    }
        //}
        ScaleGraphValues();
    }

    private void ScaleGraphValues()
    {
        // Update Axis
        float X_Interval_Value = (MaxValue.x - ZeroValue.x) / (X_Axis_Values.Count - 1);
        float CurrentIndexValue = X_Interval_Value + ZeroValue.x;
        for (int i = 0; i < X_Axis_Values.Count; i++)
        {
            if (i == 0)
            {
                X_Axis_Values[i].SetDisplay("" + ZeroValue.x);
                X_Axis_Values[i].UpdateTextDisplay();
            }
            else if (i < X_Axis_Values.Count - 1)
            {
                float RoundedCurrentValue = Mathf.Round(CurrentIndexValue * 100) / 100;
                X_Axis_Values[i].SetDisplay("" + RoundedCurrentValue);
                X_Axis_Values[i].UpdateTextDisplay();
                CurrentIndexValue += X_Interval_Value;
            }
            else
            {
                X_Axis_Values[i].SetDisplay("" + MaxValue.x);
                X_Axis_Values[i].UpdateTextDisplay();
            }
        }
        float Y_Interval_Value = (MaxValue.y - ZeroValue.y) / (Y_Axis_Values.Count - 1);
        CurrentIndexValue = Y_Interval_Value + ZeroValue.y;
        for (int i = 0; i < Y_Axis_Values.Count; i++)
        {
            if (i == 0)
            {
                Y_Axis_Values[i].SetDisplay("" + ZeroValue.y);
                Y_Axis_Values[i].UpdateTextDisplay();
            }
            else if (i < X_Axis_Values.Count - 1)
            {
                float RoundedCurrentValue = Mathf.Round(CurrentIndexValue * 100) / 100;
                Y_Axis_Values[i].SetDisplay("" + RoundedCurrentValue);
                Y_Axis_Values[i].UpdateTextDisplay();
                CurrentIndexValue += Y_Interval_Value;
            }
            else
            {
                Y_Axis_Values[i].SetDisplay("" + MaxValue.y);
                Y_Axis_Values[i].UpdateTextDisplay();
            }
        }

        // Update 0 Lines
        if (MaxValue.x > 0 && ZeroValue.x < 0)
        {
            NewZeroTransformY.gameObject.SetActive(true);
            float X_Pos = Interpolate(0, TenTenTransform.position.x, ZeroZeroTransform.position.x, MaxValue.x, ZeroValue.x);
            NewZeroTransformY.position = new Vector3(X_Pos, NewZeroTransformY.position.y, 0);
        }
        else
        {
            NewZeroTransformY.gameObject.SetActive(false);
        }

        if (MaxValue.y > 0 && ZeroValue.y < 0)
        {
            NewZeroTransformX.gameObject.SetActive(true);
            float Y_Pos = Interpolate(0, TenTenTransform.position.y, ZeroZeroTransform.position.y, MaxValue.y, ZeroValue.y);
            NewZeroTransformX.position = new Vector3(NewZeroTransformX.position.x, Y_Pos, 0);

        }
        else
        {
            NewZeroTransformX.gameObject.SetActive(false);
        }

        // Update Dots
        if (DotList.Count > 0 && ResizedGraph == true)
        {
            foreach (GraphDataPoint point in DotList)
            {
                RectTransform PointTransform = point.GetComponent<RectTransform>();
                if (PointTransform)
                {
                    float X_Pos = Interpolate(point.DataPoint.x, TenTenTransform.position.x, ZeroZeroTransform.position.x, MaxValue.x, ZeroValue.x);
                    float Y_Pos = Interpolate(point.DataPoint.y, TenTenTransform.position.y, ZeroZeroTransform.position.y, MaxValue.y, ZeroValue.y);
                    Vector3 position = new Vector3(X_Pos, Y_Pos, 0);
                    PointTransform.position = position;
                    point.Position = position;
                }
            }
            ResizedGraph = false;
        }
    }

    public void AddDataPoint(float X_Axis, float Y_Axis)
    {
        AddDataPoint(new Vector2(X_Axis, Y_Axis));
    }

    public void SetTitle(string title)
    {
        Title.SetDisplay(title);
        Title.UpdateTextDisplay();
    }

    public void SetXAxis(string title)
    {
        X_Axis.SetDisplay(title);
        X_Axis.UpdateTextDisplay();
    }

    public void SetYAxis(string title)
    {
        Y_Axis.SetDisplay(title);
        Y_Axis.UpdateTextDisplay();
    }

    public float ScaleValue(float Value, float ScaleMaxValue, float ScaleMinValue)
    {
        float Result = 0;
        Result = (Value - ScaleMinValue)  / (ScaleMaxValue - ScaleMinValue) + ScaleMinValue;
        return Result;
    }

    public float Interpolate(float Value, float InterpolateToMaxValue, float InterpolateToMinValue, float InterpolateFromMaxValue, float InterpolateFromMinValue)
    {
        float Result = 0;
        if (Value == InterpolateFromMinValue)
            Result = InterpolateToMinValue;
        else if (InterpolateFromMaxValue == InterpolateFromMinValue)
            Result = InterpolateToMinValue;
        else 
            Result = (Value - InterpolateFromMinValue) * (InterpolateToMaxValue - InterpolateToMinValue) / (InterpolateFromMaxValue - InterpolateFromMinValue) + InterpolateToMinValue;
        return Result;
    }
}
