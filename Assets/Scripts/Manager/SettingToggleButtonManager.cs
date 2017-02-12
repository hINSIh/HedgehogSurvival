using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingToggleButtonManager : MonoBehaviour {
	public Toggle bgmToggle;
	public Toggle fxSoundToggle;

	void Start() {
		SettingManager setting = Manager.Get<SettingManager>();

		bgmToggle.isOn = setting.BgmEnable;
		fxSoundToggle.isOn = setting.FxSoundEnable;
	}
}
