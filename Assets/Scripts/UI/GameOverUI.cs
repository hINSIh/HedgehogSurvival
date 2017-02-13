using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverData {
	public int survivalTime;
	public int kill;
}

public class GameOverUI : MonoBehaviour {

	public Text survivalTimeText;
	public Text killText;
	public Text coinText;

	private GameOverData data;

	void Start () {
		data = Manager.Get<GameOverData>();

		int coin = GetCoin();
		survivalTimeText.text = GetTimeFormat();
		killText.text = string.Format("{0} 마리", data.kill);
		coinText.text = string.Format("{0:n0}", coin);

		Manager.Get<CoinManager>().Deposit(coin);
		Manager.UnregisterAll();

		AchievementManager achievementManager = Manager.Get<AchievementManager>();
		achievementManager.survivalTime.Value = data.survivalTime;
		achievementManager.kill.Value = data.kill;
	}

	private string GetTimeFormat() {
		int second = data.survivalTime % 60;
		int minute = data.survivalTime / 60;

		return string.Format("{0} 분 {1} 초", GetTimeNumber(minute), GetTimeNumber(second));
	}

	private string GetTimeNumber(int value) {
		string format = "{0}";
		if (value < 10) {
			format = "0" + format;
		}

		return string.Format(format, value);
	}

	private int GetCoin() {
		int result = 0;

		result += GetCalculation(data.survivalTime, 10, 2);
		result += GetCalculation(data.kill, 15, 5);

		return result;
	}

	private int GetCalculation(int value, int divisionUnit, int weight) {
		int result = 0;

		int division = value / divisionUnit;
		for (int i = 1; i <= division; i++)
		{
			result += i * weight * divisionUnit;
		}

		if (division != 0) {
			result += division % divisionUnit * (int) Mathf.Pow(division, 1.8f);
		}

		return result;
	}
}
