using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementText : MonoBehaviour {

	private Text textUI;

	void Start () {
		textUI = GetComponent<Text>();
		AchievementManager manager = Manager.Get<AchievementManager>();

		string text = string.Format("{0}\n{1}\n{2}\n\n{3}",
									manager.survivalTime,
									manager.kill,
									manager.combo,
									manager.collectedMoney
		                           );
		textUI.text = text;

	}
}
