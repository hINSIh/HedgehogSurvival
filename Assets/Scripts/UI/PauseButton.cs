using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {
	public delegate void OnPauseEvent();
	public delegate void OnPlayEvent();

	public event OnPauseEvent OnPauseEventHandler;
	public event OnPlayEvent OnPlayEventHandler;

	public Animator animator;

	private Button button;

	void Start() {
		button = GetComponent<Button>();
	}

	public void OnClick() {
		OnPauseEventHandler();
		Time.timeScale = 0;
		animator.SetBool("Pause", true);
		button.interactable = false;
	}

	public void OnPlay() { 
		OnPlayEventHandler();
		Time.timeScale = 1;
		animator.SetBool("Pause", false);
		button.interactable = true;
	}

	public void OnMenu() {
		Time.timeScale = 1;
		animator.SetBool("Pause", false);
		Manager.Get<Player>().Health = 0;
	}
}
