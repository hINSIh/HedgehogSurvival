using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private bool isPlaying;

	public enum PlayType {
		Survival, Stage
	}

	public PlayType playType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsPlaying { 
		get { return isPlaying; }
		set { isPlaying = value; }
	}

	private void HideGame() { 
		
	}
}
