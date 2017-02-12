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
		Player.OnDamageEventListener += OnDamageEvent;
		player = Manager.Get<Player>();

		button = GetComponent<Button>();
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

	private void OnDamageEvent(PlayerDamageEvent e)
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
