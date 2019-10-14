using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

// Copyright 2019, Brady Irvine, All rights reserved
// This script handles all input from a joystick and makes it publicly accessible to other scripts
// Feel free to use it in any of your projects. but if you release source code, please comment any changes
public class MobileJoystickInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // Components
    [Header("Components")]
    [SerializeField] private Canvas canvas;                             // Joystick canvas
    [SerializeField] private RectTransform JoystickBackground = null;   // Outer ring image of joystick
    [SerializeField] private RectTransform JoystickHandle = null;       // Inner handle image of joystick

    // User input access
    public float HorizontalInput { get { return input.x; } }         // Access horizontal input
    public float VerticalInput { get { return input.y; } }           // Access vertical input

    // Joystick position past 90% on given axis
    public bool IsHoldingUp { get { return HoldingUp; } }       // Public access to HoldUp boolean
    public bool IsHoldingDown { get { return HoldingUp; } }     // Public access to HoldDown boolean
    public bool IsHoldingRight { get { return HoldingUp; } }    // Public access to HoldRight boolean
    public bool IsHoldingLeft { get { return HoldingUp; } }     // Public access to HoldLeft boolean

    // Joystick tracking
    public bool IsInUse { get { return joystickInUse; } }      // Public access to JoystickInUse boolean
    public bool IsMaxDrag { get { return joystickMaxDrag; } }  // Public access to JoystickMaxDrag boolean
    Vector2 input = Vector2.zero;
    
    [Space(8)]
    [Header("Joystick Handle Bounds")]

    [Tooltip("Handle range past this will return IsMaxDrag=true")]
    [SerializeField] private float registerMaxDrag = 0.9f;  // 90% input handling
    [SerializeField] private float deadZone = 0;            // minimum drag distance for input response

    [Space(8)]
    [Header("Joystick Functionality")]

    [SerializeField] private bool HoldingUp = false;        // VerticalInput > registerMaxDrag
    [SerializeField] private bool HoldingDown = false;      // VerticalInput < -registerMaxDrag
    [SerializeField] private bool HoldingRight = false;     // HorizontalInput > registerMaxDrag
    [SerializeField] private bool HoldingLeft = false;      // HorizontalInput < -registerMaxDrag

    [Space(8)]
    [SerializeField] private bool joystickInUse;    // User is holding finger on this joystick
    [SerializeField] private bool joystickMaxDrag;  // Joystick handle is being held down past registerMaxDrag threshold

    // Initialize components
    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    // Register User touching the screen
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // Update User input and joystick UI
    public void OnDrag(PointerEventData eventData)
    {
        // Get current position of joystick handle
        Vector2 originPoint = RectTransformUtility.WorldToScreenPoint(null, JoystickBackground.position);

        // Radius of joystick background (Max drag distance)
        Vector2 radius = JoystickBackground.rect.size / 2;

        // Finger position - original joystick handle position
        input = (eventData.position - originPoint) / (radius * canvas.scaleFactor);
        
        // Apply change in position to user Input control
        HandleInput(input.magnitude);

        // Set joystick handle position to match user input
        JoystickHandle.anchoredPosition = new Vector2(input.x * radius.x, input.y * radius.y);
    }

    protected void HandleInput(float magnitude)
    {
        // Check input is greater than minimum drag distance
        if (magnitude > deadZone)
        {
            joystickInUse = true;

            if(magnitude > 1)
            {
                // Check for all distances
                if (magnitude >= registerMaxDrag)
                {
                    joystickMaxDrag = true;
                }
                else joystickMaxDrag = false;

                // Current handle position > 90% UP
                if (VerticalInput > registerMaxDrag)
                {
                    HoldingUp = true;
                }
                else HoldingUp = false;

                // Current handle position > 90% DOWN
                if (VerticalInput < -registerMaxDrag)
                {
                    HoldingDown = true;
                }
                else HoldingDown = false;

                // Current handle position > 90% RIGHT
                if (HorizontalInput > registerMaxDrag)
                {
                    HoldingRight = true;
                }
                else HoldingRight = false;

                // Current handle position > 90% LEFT
                if (HorizontalInput < -registerMaxDrag)
                {
                    HoldingLeft = true;
                }
                else HoldingLeft = false;

                input = input.normalized;
            }
        }
        else
        {
            joystickInUse = false;
        }
    }

    // User released Finger: Reset all
    public void OnPointerUp(PointerEventData eventData)
    {
        // Holding
        joystickInUse = false;
        joystickMaxDrag = false;

        // Holding Max Dir
        HoldingLeft = false;
        HoldingRight = false;
        HoldingDown = false;
        HoldingUp = false;

        // Other Properties
        input = Vector2.zero;
        JoystickHandle.anchoredPosition = Vector2.zero;
    }
}
