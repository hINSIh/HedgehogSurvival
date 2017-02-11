using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	public Slider healthSlider;

	[Header("Status UI")]
	public Image statusImage;
	public Sprite[] statusSprites;

	private Player.HealthData data;
	private int currentHealth;
	private int currentStatus;

	void Start () {
		AbilityManager ability = Manager.Get<AbilityManager>();
		int healthAbilityLevel = ability.Get(AbilityType.Health) - 1;

		data = Manager.Get<Player>().healthData;
		data.SetAbilityLevel(healthAbilityLevel);

		healthSlider.maxValue = data.maxHealth;
		healthSlider.value = data.maxHealth;

		Health = data.maxHealth;
		SetStatusImage();
	}

	public int Health { 
		get { return currentHealth; }
		set {
			currentHealth = value;
            healthSlider.value = currentHealth;
			SetStatusImage();
		}
	}

	private void SetStatusImage() {
		float unit = (float) data.maxHealth / statusSprites.Length;
		int result = 0;

		for (int i = 1; i < statusSprites.Length; i++) {
			if (currentHealth >= unit * i) {
				continue;
			}

			result = statusSprites.Length - i;
			break;
		}

		if (currentStatus == (int) result) {
			return;
		}

		currentStatus = result;
		statusImage.sprite = statusSprites[result];
	}
}
