using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
	[System.Serializable]
	public class Counter
	{
		public float timer = 2.8f;
		public int currentCount = 0;
		public float currentTimer = 0;

		private Achievement<int> comboAchievement;

		public void Setup() {
			comboAchievement = Manager.Get<AchievementManager>().combo;
		}

		public int Update()
		{
			currentTimer = timer;
			currentCount++;

			comboAchievement.Value = currentCount;

			return currentCount;
		}

		public void Timer(float time) {
			currentTimer -= time;
			if (currentTimer <= 0) {
				currentCount = 0;
			}
		}
	}

	public Counter counter;

	public Slider timerProgressBar;
	public Animator animator;
	public Text text;

	private bool isTimer = false;
	private const string textFormat = "{0} Combo";

	void Start()
	{
		Enemy.OnEnemyDamageEventHandler += OnEnemyDamageEvent;
		Player.OnDeathEventHandler += OnPlayerDeathEvent;
		Player.OnDamageEventHandler += OnPlayerDamageEvent;

		counter.Setup();
	}

	public void Combo()
	{
		int count = counter.Update();
		UpdateUI(count);
		if (!isTimer) {
			StartCoroutine(TimerCheck());
		}
	}

	private void UpdateUI(int count) { 
		animator.SetInteger("Count", count);

		if (count > 0)
		{
			text.text = string.Format(textFormat, count);
		}
	}

	private void OnEnemyDamageEvent(EnemyDamageEvent e)
	{
		Combo();
	}

	private void OnPlayerDeathEvent(PlayerDeathEvent e) {
		StopAllCoroutines();
	}

	private void OnPlayerDamageEvent(PlayerDamageEvent e) {
		counter.currentTimer = 0;
	}

	private IEnumerator TimerCheck() {
		isTimer = true;
		float waitSec = 0.03f;
		WaitForSeconds wait = new WaitForSeconds(waitSec);

		while (counter.currentCount > 0) {
			yield return wait;
			counter.Timer(waitSec);
			timerProgressBar.value = counter.currentTimer / counter.timer;
		}
		UpdateUI(0);
		isTimer = false;
	}
}
