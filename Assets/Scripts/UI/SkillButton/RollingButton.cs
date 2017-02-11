using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingButton : MonoBehaviour, SkillButton {
	public Button chargeButton;
	public float activeEnergyRatio = 0.2f;

	private Button button;
	private Player player;
	private bool isPointerDown = false;

	void Start() {
		button = GetComponent<Button>();
		player = Manager.Get<Player>();
		player.OnEnergyChangedEventListener += OnEnergyChangeEvent;
	}

	public void OnPointerDown() {
		if (!button.interactable) {
			return;
		}

		player.State = player.stateStorage.rollingState;
		chargeButton.interactable = false;
		isPointerDown = true;
	}

	public void OnPointerUp() { 
		player.State = player.stateStorage.normalState;
		chargeButton.interactable = true;
		isPointerDown = false;

		SetInteractable(activeEnergyRatio, player.Energy);
	}

	private void OnEnergyChangeEvent(EnergyChangedEvent e) {
		if (isPointerDown)
		{
			bool canInteract = SetInteractable(0, e.toEnergy);
			if (isPointerDown && !canInteract)
			{
				OnPointerUp();
			}
		}
		else {
			SetInteractable(activeEnergyRatio, e.toEnergy);
		}
	}

	private bool SetInteractable(float minEnergyRatio, float energy) {
		float energyRatio;
		if (energy <= 0) 
		{ energyRatio = 0; } 
		else { energyRatio =  energy / player.energyData.maxEnergy; }

		bool canInteract = energyRatio > minEnergyRatio;
		button.interactable = canInteract;

		return canInteract;
	}
}
