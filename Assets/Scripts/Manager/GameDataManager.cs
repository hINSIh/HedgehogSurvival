using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
	

	public enum PlayType {
		Survival, Stage
	}

	public PlayType playType;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	//public 
}
