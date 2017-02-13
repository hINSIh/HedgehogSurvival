using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public AudioSource audioSource;

	public AudioClip damageSound;
	public AudioClip attackSound;

	void Start() {
		Player.OnDamageEventHandler += OnPlayerDamageEvent;
		Enemy.OnEnemyDamageEventHandler += OnEnemyDamageEvent;
	}

	private void OnPlayerDamageEvent(PlayerDamageEvent e) {
		audioSource.Play();
	}

	private void OnEnemyDamageEvent(EnemyDamageEvent e) { 
	
	}
}
