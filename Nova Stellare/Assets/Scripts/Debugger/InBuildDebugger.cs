using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InBuildDebugger : MonoBehaviour
{
    [SerializeField]
    private TextDisplay ConsoleDisplay;

    public void SendDebugMessege(string Debug)
    {
        ConsoleDisplay.SetDisplay(Debug, 10, 50);
        ConsoleDisplay.UpdateTextDisplay();
    }
}
