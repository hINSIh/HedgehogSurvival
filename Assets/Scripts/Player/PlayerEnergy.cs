using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour {

	public float maxEnergy = 200;
	public float chargeSpeed = 70;
	public float spendSpeed = 50;
	public Slider energySlider;
	public Animator animator;

	[Header("Button")]
	public Button chargeButton;
	public Button transformButton;

	[Header("Upgrade")]
	public float increaseEnergy = 20;
	public float increaseChargeSpeed = 15;

	private float currentEnergy;
	private bool chargeToggle;
	private bool rolling;

	// Use this for initialization
	void Start () {
		AbilityManager ability = Manager.Get<AbilityManager>();
		int energyAbility = ability.Get(AbilityType.Energy) - 1;

		maxEnergy += increaseEnergy * energyAbility;
		chargeSpeed += increaseChargeSpeed * energyAbility;

		energySlider.maxValue = maxEnergy;
		energySlider.value = maxEnergy;

		currentEnergy = maxEnergy;
	}

	void FixedUpdate() {
		if (rolling) {
			Spend(spendSpeed * Time.fixedDeltaTime);
		} else if (chargeToggle) {
			Charge(chargeSpeed * Time.fixedDeltaTime);
		}

		SetButtons();
	}

	public void Spend(float value) {
		if (value < 0) {
			return;
		}

		AddEnergy(-value);
	}

	public void Charge(float value) { 
		if (value < 0) {
			return;
		}

		AddEnergy(value);
	}

	public void ChargeToggle(bool toggle) {
		chargeToggle = toggle;
		animator.SetBool("Charge", toggle);
	}

	public bool IsRolling()
	{
		return rolling;
	}

	public bool IsChargeToggle() {
		return chargeToggle;
	}

	public float GetEnergy() {
		return currentEnergy;
	}

	public void SetRolling(bool value)
	{
		if (!transformButton.interactable) {
			return;
		}

		rolling = value;
		animator.SetBool("Rolling", value);
		if (value) {
			animator.SetBool("Damage", false);
		}
	}

	private void SetButtons() {
		if (currentEnergy <= 0) {
			SetRolling(false);
			transformButton.interactable = false;
		} else if (currentEnergy >= 10) { 
			transformButton.interactable = true;
		}
	}

	private void AddEnergy(float value) {
		currentEnergy += value;
		currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

		energySlider.value = currentEnergy;
	}
}
