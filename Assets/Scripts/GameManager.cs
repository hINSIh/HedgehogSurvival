using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private bool isPause = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Pause { 
		get { return isPause; }
		set {
			isPause = value;
			Time.timeScale = isPause ? 0 : 1;
		}
	}
}
