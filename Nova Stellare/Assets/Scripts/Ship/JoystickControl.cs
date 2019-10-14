using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickControl : MonoBehaviour
{
    [SerializeField]
    private RectTransform CenterTransform; // Transform that the joystick will try to get back too
    private RectTransform MyTransform;

    
    public bool IsPressed = false;

    [SerializeField]
    private float MaxMovementRange = 1;
    [SerializeField]
    private float DeadZone = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyTransform.position != CenterTransform.position)
        {
            if (!IsPressed)
            {
                MyTransform.position = CenterTransform.position;
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    IsPressed = false;
                }
                else if (Input.GetMouseButtonUp(0))
                {

                }
            }
        }
    }

    public void TogglePressed(bool chooseState = false, bool State = true)
    {
        if (chooseState)
        {
            IsPressed = State;
        }
        else
        {
            IsPressed = !IsPressed;
        }
    }

    public void ButtonPress()
    {
        IsPressed = true;
    }

    private bool GetInputUp()
    {
        bool result = false;
        if (Input.GetMouseButtonUp(0))
        {
            result = true;
        }
        //else if (Input.GetTouch(0).position )
        //{

        //}

        return result;
    }
}
