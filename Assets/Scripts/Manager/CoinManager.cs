using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoinChangedListner {
	void OnCoinChangedEvent(int coin);
}

public class CoinManager : MonoBehaviour {

	private const string prefsPath = "coin";
	private int coin;

	private List<ICoinChangedListner> eventListeners = new List<ICoinChangedListner>();

	void Awake() {
		coin = 100000;
	}

	public int Coin {
		get { return coin; }
		set {
			PlayerPrefs.SetInt(prefsPath, value);
			coin = value;
			CallChangedEvents();
		}
	}

	public bool Deposit(int value) {
		if (value < 0) {
			return false;
		}

		coin += value;
		CallChangedEvents();
		return true;
	}

	public bool Withdraw(int value) {
		if (coin - value < 0) {
			return false;
		}

		coin -= value;
		CallChangedEvents();
		return true;
	}

	public void AddChangedEventListener(ICoinChangedListner listener) {
		eventListeners.Add(listener);
	}

	public void RemoveChangedEventListener(ICoinChangedListner listener) {
		eventListeners.Remove(listener);
	}

	private void CallChangedEvents() {
		foreach (ICoinChangedListner listener in eventListeners) {
			listener.OnCoinChangedEvent(coin);
		}
	}
}
