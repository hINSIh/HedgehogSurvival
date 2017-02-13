using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
	public AudioSource audioSource;

	void Start() {
		SettingManager manager = Manager.Get<SettingManager>();
		Toggle(manager.BgmEnable);
	}

	public void Toggle(bool value) {
		if (value)
		{
			audioSource.UnPause();
		}
		else {
			audioSource.Pause();
		}
	}
}
