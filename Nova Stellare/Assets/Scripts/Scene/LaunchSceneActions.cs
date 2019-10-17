using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchSceneActions : MonoBehaviour
{
    [SerializeField]
    private float BeginingWaitTime = 2f;
    [SerializeField]
    private float FadeInTime = 1f;
    [SerializeField]
    private float WaitTime = 1f;
    [SerializeField]
    private float FadeOutTime = 1f;
    private float PreTimer = 0f;
    private float timer = 0f;
    private float Fade = 0f;
    private bool FadedIn = false;
    private bool FadedOut = false;

    // Components for scene actions
    [SerializeField]
    private SceneManagement SceneLoader;
    [SerializeField]
    private RawImage FadeImage;
    [SerializeField]
    private ImageDisplay SceneDisplay;


    // Temp Values
    Graph graph;
    private void Start()
    {
        //StartCoroutine("DisplayLogo");

        // if you find a graph object
        graph = FindObjectOfType<Graph>();
        if (graph)
        {
            graph.SetTitle("Fade");
            graph.SetXAxis("Time (sec)");
            graph.SetYAxis("Transparency");
        }
    }

    private void FixedUpdate()
    {
        if (graph && timer <= FadeInTime + FadeOutTime + WaitTime)
        {
            float time = Mathf.Round(timer * 100) / 100;
            float fade = Mathf.Round(Fade * 100) / 100;
            graph.AddDataPoint(time, fade);
        }
        if (PreTimer < BeginingWaitTime)
        {
            PreTimer += Time.fixedDeltaTime;
        }
        else if (timer <= FadeInTime + FadeOutTime + WaitTime)
        {
            timer += Time.fixedDeltaTime;
            if (timer < FadeInTime && !FadedIn)
            {
                FadedIn = true;
                //Debug.Log("Faded In");
                FadeImage.CrossFadeAlpha(0, FadeInTime, true);
            }
            else if (timer >= FadeInTime + WaitTime && !FadedOut)
            {
                FadedOut = true;
                FadeImage.CrossFadeAlpha(1, FadeOutTime, true);
            }
            Fade = FadeImage.color.a;
        }
        else
        {
            SceneLoader.LoadSceneByIndex(1);
        }
    }

    private IEnumerator DisplayLogo()
    {
        while (timer <= FadeInTime + FadeOutTime + WaitTime)
        {
            timer += Time.deltaTime;
            // Fade In
            if (timer < FadeInTime)
            {
                Color temp = FadeImage.color;
                Fade = 255 - 255 * timer / FadeInTime;
                temp.a -= 255 * timer / FadeInTime;
                FadeImage.color = temp;
            }
            // Wait - Do Nothing -

            // Fade Out
            if (timer >= FadeInTime + WaitTime && timer < FadeInTime + FadeOutTime + WaitTime)
            {
                timer += Time.deltaTime;
                Color temp = FadeImage.color;
                Fade = 255f * (timer - FadeInTime - WaitTime) / FadeOutTime;
                temp.a = 255f * (timer - FadeInTime - WaitTime) / FadeOutTime;
                FadeImage.color = temp;
            }

            yield return null;
        }

        // Load next Scene
        SceneLoader.LoadSceneByIndex(1);
    }
}
