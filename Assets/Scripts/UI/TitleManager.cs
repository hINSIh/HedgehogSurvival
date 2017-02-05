using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TitleSchedule {
	public string message;
	public readonly float typingDelay;
	public readonly float destoryDelay;

	public TitleSchedule(string message, float typingDelay, float destoryDelay) {
		this.message = message;
		this.typingDelay = typingDelay;
		this.destoryDelay = destoryDelay;
	}
}

public class TitleManager : MonoBehaviour {

	private Queue<TitleSchedule> schedules = new Queue<TitleSchedule>();
	private bool running;

	private Animator animator;
	private Text title;

	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
		title = transform.GetChild(0).GetComponent<Text>();
		title.text = null;

		Manager.RegisterManager(this);
	}

	public void AddSchedule(TitleSchedule titleSchedule) {
		schedules.Enqueue(titleSchedule);
	}

	public IEnumerator Run() {
		if (running) {
			throw new UnityException("Alerady running.");
		}

		running = true;
		animator.SetBool("Playing", true);
		yield return new WaitForSeconds(0.5f);

		TitleSchedule schedule;
		while (schedules.Count > 0)
		{
			schedule = schedules.Dequeue();
			yield return StartTyping(schedule);
		}

		animator.SetBool("Playing", false);
		yield return new WaitForSeconds(0.5f);
		running = false;
	}

	private IEnumerator StartTyping(TitleSchedule schedule) {
		char[] charArray = schedule.message.ToCharArray();
		string typingMessage = "";
		WaitForSeconds typingDelay = new WaitForSeconds(schedule.typingDelay);

		foreach (char character in charArray) {
			typingMessage += character;
			if (character == ' ') {
				continue;
			}

			title.text = typingMessage;
			yield return typingDelay;
		}

		yield return new WaitForSeconds(schedule.destoryDelay);

		for (int i = charArray.Length - 1; i >= 0; i--) {
			if (charArray[i] == ' ') {
				continue;
			}

			title.text = typingMessage.Remove(i);
			yield return typingDelay;
		}

		title.text = "";
		yield return new WaitForSeconds(0.1f);
	}
}
