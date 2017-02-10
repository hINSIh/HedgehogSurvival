using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour {

    public AudioSource audio;

	public void ButtonOnClick()
    {
        this.audio.Stop();
        this.audio.Play();
    }
}
