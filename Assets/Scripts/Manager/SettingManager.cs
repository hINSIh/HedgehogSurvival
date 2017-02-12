using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour {
	private class Option<T>
	{
		public T value;
		public readonly string prefsNode;

		public Option(string prefsNode) {
			this.prefsNode = prefsNode;
		}
	}

	private Option<bool> bgmEnableOption = new Option<bool>("setting.bgmEnable");
	private Option<bool> fxSoundEnableOption = new Option<bool>("setting.fxSoundEnable");

	void Awake() {
		if (!PlayerPrefs.HasKey(bgmEnableOption.prefsNode)) {
			PlayerPrefs.SetInt(bgmEnableOption.prefsNode, 1);
			PlayerPrefs.SetInt(fxSoundEnableOption.prefsNode, 1);
		}

		bool bgmEnable = 
			PlayerPrefs.GetInt(bgmEnableOption.prefsNode) != 0;
		bool fxSoundEnable =
			PlayerPrefs.GetInt(fxSoundEnableOption.prefsNode) != 0;
		
		bgmEnableOption.value = bgmEnable;
		fxSoundEnableOption.value = fxSoundEnable;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool BgmEnable { 
		get { return bgmEnableOption.value; }
		set { bgmEnableOption.value = value; }
	}

	public bool FxSoundEnable
	{
		get { return fxSoundEnableOption.value; }
		set { fxSoundEnableOption.value  = value; }
	}
}
