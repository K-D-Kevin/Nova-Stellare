using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField]
    private TextDisplay SceneLoadingOperationText;
    [SerializeField]
    private GameObject SceneLoadObject;
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

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine("AsyncSceneLoad", index);
    }

    private IEnumerator AsyncSceneLoad(int index)
    {
        AsyncOperation SceneLoad = SceneManager.LoadSceneAsync(index);
        SceneLoadObject.gameObject.SetActive(true);
        while (!SceneLoad.isDone)
        {
            float progress = Mathf.Round(SceneLoad.progress * 1000) / 10;
            SceneLoadingOperationText.SetDisplay("Loading " + progress + "%", 10, 50);
            SceneLoadingOperationText.UpdateTextDisplay();
            yield return null;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
