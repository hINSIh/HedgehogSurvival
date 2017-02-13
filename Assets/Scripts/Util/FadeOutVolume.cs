using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutVolume : MonoBehaviour {

    public AudioSource audio;

	public void Click()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        float j = 1f;
        for (float i = 0; i < 10; i ++)
        {
            if (j > 0)
                j -= 0.1f;
            audio.volume = j;
            yield return null;
        }
    }
}
