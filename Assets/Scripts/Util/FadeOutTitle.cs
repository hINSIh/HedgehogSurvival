using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutTitle : MonoBehaviour
{

    public int sceneBuildIndex;
    public Image fadeOutImage;

    public void GetTouch()
    { 
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Color color = fadeOutImage.color;
        for (float i = 0; i < 255; i += 15)
        {
            color.a = i / 255;
            fadeOutImage.color = color;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex);
    }
}
