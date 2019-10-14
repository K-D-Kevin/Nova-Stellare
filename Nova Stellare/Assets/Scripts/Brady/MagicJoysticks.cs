using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

// Copyright 2019, Brady Irvine, All rights reserved
// This script hides and shows a joystick at the touch location based on the side of the screen you click
// Feel free to use it in any of your projects. but if you release source code, please comment any changes
public class MagicJoysticks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // Flipped because layout is landscape
    private float SCREEN_WIDTH = Screen.width;
    private float SCREEN_HEIGHT = Screen.height;

    public GameObject joystickLeft;
    public GameObject joystickRight;

    private bool joystickLeftInUse;
    private bool joystickRightInUse;

    MobileJoystickInput joystickLeftInput;
    MobileJoystickInput joystickRightInput;

    private Vector3 joystickLeftOriginalPosition;
    private Vector3 joystickRightOriginalPosition;

    private void Start()
    {
        SCREEN_WIDTH = Screen.width;
        SCREEN_HEIGHT = Screen.height;

        joystickLeftInput = joystickLeft.GetComponent<MobileJoystickInput>();
        joystickRightInput = joystickRight.GetComponent<MobileJoystickInput>();

        joystickLeftOriginalPosition = joystickLeft.transform.position;
        joystickRightOriginalPosition = joystickRight.transform.position;
    }

    // Show joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        // Left joystick
        if (eventData.pressPosition.x < SCREEN_WIDTH/2)
        {
            joystickLeft.transform.position = eventData.pressPosition;
            joystickLeft.SetActive(true);
        }

        // Right joystick
        else
        {
            joystickRight.transform.position = eventData.pressPosition;
            joystickRight.SetActive(true);
        }
    }

    // Handle Input
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pressPosition.x < SCREEN_WIDTH / 2)
        {
            joystickLeftInput.OnDrag(eventData);
        }

        else
        {
            joystickRightInput.OnDrag(eventData);
        }
    }

    // Hide joystick
    public void OnPointerUp(PointerEventData eventData)
    {
        // Left joystick
        if (eventData.pressPosition.x < SCREEN_WIDTH / 2)
        {
            joystickLeft.SetActive(false);
            joystickLeftInput.OnPointerUp(eventData);
        }

        // Right joystick
        else
        {
            joystickRight.SetActive(false);
            joystickRightInput.OnPointerUp(eventData);
        }
    }

    private void OnEnable()
    {
        joystickLeft.SetActive(false);
        joystickRight.SetActive(false);

        if(!joystickLeftInput) joystickLeft.GetComponent<MobileJoystickInput>();
        if(!joystickRightInput) joystickRight.GetComponent<MobileJoystickInput>();
    }

    private void OnDisable()
    {
        joystickLeft.SetActive(true);
        joystickRight.SetActive(true);

         joystickLeft.transform.position = joystickLeftOriginalPosition;
         joystickRight.transform.position = joystickRightOriginalPosition;
    }
}
