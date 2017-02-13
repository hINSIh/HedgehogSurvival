using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingToggleButtonManager : MonoBehaviour {
	public Toggle bgmToggle;
	public Toggle fxSoundToggle;

	private SettingManager setting;

	void Start() {
		setting = Manager.Get<SettingManager>();

		bgmToggle.isOn = setting.BgmEnable;
		fxSoundToggle.isOn = setting.FxSoundEnable;

		bgmToggle.onValueChanged.AddListener(
			delegate { setting.BgmEnable = bgmToggle.isOn; }
		);

		fxSoundToggle.onValueChanged.AddListener(
			delegate { setting.FxSoundEnable = fxSoundToggle.isOn; }
		);
	}
}
