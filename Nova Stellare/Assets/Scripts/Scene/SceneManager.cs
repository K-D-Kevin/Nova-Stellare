using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    // When the game closes - save settings / game / etc
    public void OnApplicationQuit()
    {
        
    }

    // When the game pauses - save settings / game / etc
    public void OnApplicationPause(bool pause)
    {
        
    }
}
