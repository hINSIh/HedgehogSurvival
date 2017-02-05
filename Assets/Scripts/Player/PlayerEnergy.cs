using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour {

	public float maxEnergy = 100;
	public float chargeSpeed = 1;
	public Slider energySlider;
	public Animator animator;

	[Header("Button")]
	public Button chargeButton;
	public Button transformButton;

	[Header("Upgrade")]
	public float increaseEnergy;
	public float increaseChargeSpeed;

	private float currentEnergy;
	private bool chargeToggle;

	// Use this for initialization
	void Start () {
		energySlider.maxValue = maxEnergy;
		//energySlider.value = maxEnergy;

		//currentEnergy = maxEnergy;
	}

	void FixedUpdate() {
		if (!chargeToggle) {
			return;
		}

		Charge(chargeSpeed * Time.fixedDeltaTime);
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

	public bool IsChargeToggle() {
		return chargeToggle;
	}

	private void AddEnergy(float value) {
		currentEnergy += value;
		Debug.Log(currentEnergy);
		currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

		energySlider.value = currentEnergy;
	}
}
