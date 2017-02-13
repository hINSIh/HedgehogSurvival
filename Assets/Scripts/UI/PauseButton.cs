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
	public Text countText;

	private Button button;

	void Awake() {
		Manager.RegisterManager(this);
		button = GetComponent<Button>();
	}

	public void OnClick() {
		OnPauseEventHandler();
		Time.timeScale = 0;
		animator.SetBool("Pause", true);
		button.interactable = false;
	}

	public void OnPlay()
	{
		if (animator.GetBool("Count"))
		{
			return;
		}

		StartCoroutine(Count());
	}
	public void OnMenu() {
		Time.timeScale = 1;
		animator.SetBool("Pause", false);
		Manager.Get<Player>().Health = 0;
	}

	private IEnumerator Count() {
		animator.SetBool("Count", true);
		countText.text = "3";
		yield return new WaitForSecondsRealtime(1.5f);
		for (int i = 2; i >= 1; i--) {
			countText.text = i.ToString();
			yield return new WaitForSecondsRealtime(1f);
		}

		Time.timeScale = 1;
		animator.SetBool("Pause", false);
		animator.SetBool("Count", false);
		OnPlayEventHandler();
		button.interactable = true;
	}
}
