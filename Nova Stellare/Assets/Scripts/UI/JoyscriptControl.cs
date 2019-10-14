using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyscriptControl : MonoBehaviour
{
    [SerializeField]
    private RectTransform CenterTransform; // Transform that the joystick will try to get back too
    private RectTransform MyTransform;


    public bool IsPressed = false;

    [SerializeField]
    private float MaxMovementRange = 1;
    [SerializeField]
    private float DeadZone = 0.1f;
    private float JoystickDistance = 0;
    private Vector3 JoystickDisplacement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<RectTransform>();
        MyTransform.position = CenterTransform.position;
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

    public void StartFollow(PlayerShip ship)
    {
        IsPressed = true;
        StartCoroutine("Following", ship);
    }

    private IEnumerator Following(PlayerShip ship)
    {
        while (IsPressed)
        {
            Vector2 ScreenPoint = Camera.main.WorldToScreenPoint(MyTransform.position);
            //FindObjectOfType<InBuildDebugger>().SendDebugMessege("Touch Position Joystick / Mouse: " + ScreenPoint + " / " + ship.TouchScreenLocation + ", Position: " + MyTransform.position);
            Vector2 TouchJoystickDirection = ship.TouchScreenLocation - ScreenPoint;

            MyTransform.Translate(TouchJoystickDirection * Time.deltaTime);
            JoystickDisplacement = MyTransform.position - CenterTransform.position;
            JoystickDistance = Vector3.Magnitude(JoystickDisplacement);
            if (JoystickDistance > MaxMovementRange)
            {
                MyTransform.position = CenterTransform.position + JoystickDisplacement.normalized * MaxMovementRange;
            }
            //MyTransform.position = ship.TouchWorldLocation;
            yield return null;
        }

        MyTransform.position = CenterTransform.position;
    }

    public Vector2 GetMovementRatio()
    {
        Vector2 Ratio = Vector2.zero;

        if (IsPressed)
        {
            if (DeadZone < JoystickDistance)
            {
                float X_Top = DeadZone > Mathf.Abs(JoystickDisplacement.x) ? 0 : JoystickDisplacement.x < 0 ? JoystickDisplacement.x + DeadZone : JoystickDisplacement.x - DeadZone;
                float Y_Top = DeadZone > Mathf.Abs(JoystickDisplacement.y) ? 0 : JoystickDisplacement.y < 0 ? JoystickDisplacement.y + DeadZone : JoystickDisplacement.y - DeadZone;

                float X_Ratio = X_Top / (JoystickDistance - DeadZone);
                float Y_Ratio = Y_Top / (JoystickDistance - DeadZone);

                Ratio = new Vector2(X_Ratio, Y_Ratio);
            }
        }

        return Ratio;
    }
}
