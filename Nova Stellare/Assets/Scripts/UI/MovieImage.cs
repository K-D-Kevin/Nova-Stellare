using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieImage : MonoBehaviour
{

    public enum UiMoveDirection
    {
    Left = 0,
    Right = 1,
    Down = 2,
    Up = 3,
    }

    [SerializeField]
    private UiMoveDirection DirectionToMove;

    [SerializeField]
    private RectTransform MyTransform;

    [SerializeField]
    [Range(0, Mathf.Infinity)]
    private float TimeToMove = 0.5f;

    // Variables
    private float CurrentTime = 0;

    public void Move()
    {
        //Debug.Log("Move Hit");
        Vector3 SPD = Vector3.one;
        if (TimeToMove > 0)
        {
            if (DirectionToMove == UiMoveDirection.Down)
            {
                SPD = Vector3.down * Screen.height * Time.deltaTime / TimeToMove;
                //Debug.Log("Down Hit hit");
                StartCoroutine("IE_Move", SPD);
            }
            else if (DirectionToMove == UiMoveDirection.Up)
            {
                SPD = Vector3.up * Screen.height * Time.deltaTime / TimeToMove;
                StartCoroutine("IE_Move", SPD);
            }
            else if (DirectionToMove == UiMoveDirection.Left)
            {
                SPD = Vector3.left * Screen.width * Time.deltaTime / TimeToMove;
                //Debug.Log("Left hit");
                StartCoroutine("IE_Move", SPD);
            }
            else if (DirectionToMove == UiMoveDirection.Right)
            {
                SPD = Vector3.right * Screen.width * Time.deltaTime / TimeToMove;
                StartCoroutine("IE_Move", SPD);
            }
        }
        else
        {
            StartCoroutine("IE_Move", SPD);
        }
    }

    private IEnumerator IE_Move(Vector3 Speed)
    {
        //Debug.Log("IE_Move Hit");
        while (CurrentTime < TimeToMove)
        {
            MyTransform.Translate(Speed);
            //Debug.Log("Time: " + CurrentTime + ", Speed: " + Speed);
            CurrentTime += Time.fixedDeltaTime;
            yield return null;
        }
        // Make sure the loading screen is off
        MyTransform.Translate(Screen.width * 2 * Speed.normalized);
    }

    public void ResetMove()
    {
        CurrentTime = 0;
        MyTransform.localPosition = Vector3.zero;
    }
}
