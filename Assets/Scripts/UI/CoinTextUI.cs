using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextUI : MonoBehaviour, ICoinChangedListner {

	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		StartCoroutine(DelayStart());
	}

	void OnDestroy()
	{
		Manager.Get<CoinManager>().RemoveChangedEventListener(this);
	}

	IEnumerator DelayStart() {
		yield return null;
		OnCoinChangedEvent(Manager.Get<CoinManager>().Coin);
		Manager.Get<CoinManager>().AddChangedEventListener(this);
	}

	public void OnCoinChangedEvent(int value) { 
		text.text = string.Format("{0:n0}", value);
	}
}
