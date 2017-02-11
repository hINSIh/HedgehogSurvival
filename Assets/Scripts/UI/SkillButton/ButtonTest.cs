using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour {

	public ChargeButton chargeButton;
	public RollingButton rollingButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SetButton(KeyCode.A, rollingButton);
		SetButton(KeyCode.D, chargeButton);
	}

	private void SetButton(KeyCode key, SkillButton button) {
		if (Input.GetKeyDown(key))
		{
			button.OnPointerDown();
		}
		else if (Input.GetKeyUp(key)) {
			button.OnPointerUp();
		}
	}
}
