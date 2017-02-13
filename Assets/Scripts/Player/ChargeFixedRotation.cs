using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFixedRotation : MonoBehaviour {
	public Animator playerAnimator;
	public string stateTag = "Charge";

	// Use this for initialization
	void Start () {
		Player.OnStateChangedEventHandler += OnStateChangedEvent;
	}

	void Destory() {
		StopAllCoroutines();
	}

	private void OnStateChangedEvent(PlayerStateChangedEvent e) {
		if (EqualsState(e.changedState, typeof(ChargeState)))
		{
			StartCoroutine(ChargeIn());
		}
	}

	private bool EqualsState(IPlayerState state1, System.Type type) {
		return state1.GetType() == type;
	}

	private bool IsCharging() {
		return playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag(stateTag);
	}

	private IEnumerator ChargeIn() {
		if (!IsCharging())
		{
			while (!IsCharging())
			{
				yield return null;
			}
		}

		FixRotation(true);

		while (IsCharging())
		{
			yield return null;
		}

		FixRotation(false);
	}

	private void FixRotation(bool value) {
		if (value) {
			transform.localEulerAngles = -transform.parent.eulerAngles;
		} else {
			transform.localRotation = Quaternion.identity;
		}
	}
}
