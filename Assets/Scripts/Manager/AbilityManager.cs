using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {
	Damage, Health, Energy, RotateSpeed
}

public class AbilityManager : MonoBehaviour {

	[Serializable]
	public struct MaximumValue {
		public AbilityType type;
		public int maximum;
	}

	public MaximumValue[] maximumValues;

	private Dictionary<AbilityType, int> abilitiesMaximum = new Dictionary<AbilityType, int>();
	private Dictionary<AbilityType, int> abilities = new Dictionary<AbilityType, int>();

	private const string prefsPrefix = "level.";

	void Awake() {
		LoadLevels();
		InitMaximumValues();
		Reset();
	}

	public int Get(AbilityType type)
	{
		return abilities[type];
	}

	public int GetMaximum(AbilityType type) {
		return abilitiesMaximum[type];
	}

	public void Set(AbilityType type, int value) {
		value = Mathf.Min(value, GetMaximum(type));
		PlayerPrefs.SetInt(prefsPrefix + type, value);
		abilities[type] = value;
	}

	public void Save() {
		PlayerPrefs.Save();
	}

	public void Reset() {
		foreach (AbilityType type in Enum.GetValues(typeof(AbilityType))) {
			Set(type, 1);
		}
	}

	private void LoadLevels() {
		foreach (AbilityType type in Enum.GetValues(typeof(AbilityType))) {
			int level = PlayerPrefs.GetInt(prefsPrefix + type);
			abilities.Add(type, Mathf.Max(1, level));
		}
	}

	private void InitMaximumValues() {
		foreach (MaximumValue range in maximumValues) {
			abilitiesMaximum.Add(range.type, range.maximum);
		}
	}
}
