using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCam : MonoBehaviour
{
    public enum MoveDirection
    {
        left = 0,
        right = 1,
        top = 2,
        bottom = 3,
    }

    [SerializeField]
    private Camera MyCam;

    private PlayerShip MyShip;
    private Transform MyTransform;

    // If the ship is moving hard or not
    private bool MoveHardHorizontal = false;
    public bool CameraMovingHardHorizontal
    {
        get
        {
            return MoveHardHorizontal;
        }
        set
        {
            MoveHardHorizontal = value;
        }
    }
    private bool MoveHardVertical = false;
    // If the camera can only be moved once per update
    private bool MovedHorizontal = false;
    private bool MovedVertical = false;
    public bool CameraMovingHardVertical
    {
        get
        {
            return MoveHardVertical;
        }
        set
        {
            MoveHardVertical = value;
        }
    }
    private void Start()
    {
        MyShip = FindObjectOfType<PlayerShip>();
        MyTransform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // Resets the moved so that they can be applied next update
        MovedHorizontal = false;
        MovedVertical = false;
    }

    public void Move(MoveDirection direction)
    {

        if (direction == MoveDirection.left || direction == MoveDirection.right)
        {
            if (!MovedHorizontal)
            {
                MovedHorizontal = true;
                if (MoveHardHorizontal)
                {
                    float AppliedSpeed = 0;
                    if (direction == MoveDirection.left && MyShip.GetCurrentVelocity.x < 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.x;
                    else if (direction == MoveDirection.right && MyShip.GetCurrentVelocity.x > 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.x;
                    MyTransform.Translate(AppliedSpeed * Vector3.right);
                }
                else
                {
                    float AppliedSpeed = 0;
                    if (direction == MoveDirection.left && MyShip.GetCurrentVelocity.x < 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.x;
                    else if (direction == MoveDirection.right && MyShip.GetCurrentVelocity.x > 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.x;
                    MyTransform.Translate(AppliedSpeed * Vector3.right / 2);
                }
            }
        }
        else if (direction == MoveDirection.top || direction == MoveDirection.bottom)
        {
            if (!MovedVertical)
            {
                MovedVertical = true;
                if (MoveHardVertical)
                {
                    float AppliedSpeed = 0;
                    if (direction == MoveDirection.bottom && MyShip.GetCurrentVelocity.y < 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.y;
                    else if (direction == MoveDirection.top && MyShip.GetCurrentVelocity.y > 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.y;
                    MyTransform.Translate(AppliedSpeed * Vector3.up);
                }
                else
                {
                    float AppliedSpeed = 0;
                    if (direction == MoveDirection.bottom && MyShip.GetCurrentVelocity.y < 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.y;
                    else if (direction == MoveDirection.top && MyShip.GetCurrentVelocity.y > 0)
                        AppliedSpeed = MyShip.GetCurrentVelocity.y;
                    MyTransform.Translate(AppliedSpeed * Vector3.up / 2);
                }
            }
        }
    }
}
