using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour {
	public Slider energySlider;

	private Player.EnergyData data;
	private float currentEnergy;

	void Start () {
		AbilityManager ability = Manager.Get<AbilityManager>();
		int energyAbilityLevel = ability.Get(AbilityType.Energy) - 1;

		data = Manager.Get<Player>().energyData;
		data.SetAbilityLevel(energyAbilityLevel);

		energySlider.maxValue = data.maxEnergy;
		energySlider.value = data.maxEnergy;

		currentEnergy = data.maxEnergy;
	}

	public float Energy
	{
		get { return currentEnergy; }
		set
		{
			currentEnergy = value;
			currentEnergy = Mathf.Clamp(currentEnergy, 0, data.maxEnergy);

			energySlider.value = value;
		}
	}
}
