using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour {

	public Manager managerPrefab;

	void Awake () {
		try {
			Manager.Get<CoinManager>();
		} catch {
			Instantiate(managerPrefab);
		}
	}
}
