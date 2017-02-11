using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeButton : MonoBehaviour, SkillButton {
	private Button button;
	private Player player;
	private bool isPointerDown = false;

	void Start()
	{
		button = GetComponent<Button>();
		player = Manager.Get<Player>();
		player.OnDamageEventListener += OnDamageEvent;
	}

	public void OnPointerDown()
	{
		if (!CanCharge()) {
			return;
		}

		player.State = player.stateStorage.chargeState;
		isPointerDown = true;
	}

	public void OnPointerUp()
	{
		player.State = player.stateStorage.normalState;
		isPointerDown = false;
	}

	private void OnDamageEvent(DamageEvent e)
	{
		if (isPointerDown)
		{
			OnPointerUp();
		}
	}

	private bool CanCharge() {
		return player.State == player.stateStorage.normalState &&
			         player.CanDamage() && button.interactable;
	}
}
