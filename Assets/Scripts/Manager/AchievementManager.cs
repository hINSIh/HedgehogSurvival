using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour {
	public Achievement<int> survivalTime;
	public Achievement<int> kill;
	public Achievement<int> combo;
	public Achievement<int> collectedMoney;

	private struct IntegerSavable : ISavable<int> {
		public void Save(string prefsNode, int value) {
			PlayerPrefs.SetInt(prefsNode, value);
		}

		public int Get(string prefsNode) {
			if (PlayerPrefs.HasKey(prefsNode))
			{
				return PlayerPrefs.GetInt(prefsNode);
			}
			return 0;
		}
	}

	private struct TextFormat : AchievementFormat<int>
	{
		private string format;

		public TextFormat(string format) {
			this.format = format;
		}

		public string Format(int value) {
			return string.Format(format, value);
		}
	}

	private struct TimeFormat : AchievementFormat<int> { 
		public string Format(int value)
		{
			int second = value % 60;
			int minute = value / 60;

			return string.Format("{0} 분 {1} 초", GetTimeNumber(minute), GetTimeNumber(second));
		}

		private string GetTimeNumber(int value)
		{
			string format = "{0}";
			if (value < 10)
			{
				format = "0" + format;
			}

			return string.Format(format, value);
		}
	}

	void Awake() {
		survivalTime =
		new Achievement<int>(
			"achievement.survivaltime",
			new IntegerSavable(),
			new TimeFormat()
		);

		kill =
			new Achievement<int>(
				"achievement.kill",
				new IntegerSavable(),
				new TextFormat("{0:n0}")
			);

		combo =
			new Achievement<int>(
				"achievement.combo",
				new IntegerSavable(),
				new TextFormat("{0:n0}")
			);

		collectedMoney =
			new Achievement<int>(
				"achievement.collectedMoney",
				new IntegerSavable(),
				new TextFormat("{0:n0}")
			);
	}
}
