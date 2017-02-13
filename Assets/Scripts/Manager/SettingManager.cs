using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable<T>
{
	void Save(string prefsNode, T value);

	T Get(string prefsNode);
}

public class Option<T>
{
	protected T value;
	protected string prefsNode;
	protected ISavable<T> savable;

	public Option(string prefsNode, ISavable<T> savable)
	{
		this.prefsNode = prefsNode;
		this.savable = savable;
		value = savable.Get(prefsNode);
	}

	public virtual T Value
	{
		get { return value; }
		set
		{
			this.value = value;
			savable.Save(prefsNode, value);
		}
	}
}
public class SettingManager : MonoBehaviour {
	private class BoolSavable : ISavable<bool> {
		public void Save(string prefsNode, bool value) {
			PlayerPrefs.SetInt(prefsNode, value ? 1 : 0);
		}

		public bool Get(string prefsNode) {
			if (PlayerPrefs.HasKey(prefsNode))
			{
				return PlayerPrefs.GetInt(prefsNode) != 0;
			}
			return true;
		}
	}

	private Option<bool> bgmEnableOption;
	private Option<bool> fxSoundEnableOption;

	void Awake() {
		ISavable<bool> boolSavable = new BoolSavable();

		bgmEnableOption = new Option<bool>("setting.bgmEnable", boolSavable);
		fxSoundEnableOption = new Option<bool>("setting.fxSoundEnable", boolSavable);
	}

	public bool BgmEnable { 
		get { return bgmEnableOption.Value; }
		set { bgmEnableOption.Value = value; }
	}

	public bool FxSoundEnable
	{
		get { return fxSoundEnableOption.Value; }
		set { fxSoundEnableOption.Value = value; }
	}
}
