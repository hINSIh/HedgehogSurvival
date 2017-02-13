using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	[System.Serializable]
	public class AudioData {
		public AudioSource audioSource;
		public float minPitch = 0.9f;
		public float maxPitch = 1.1f;

		public void SetPitch() {
			audioSource.pitch = Random.Range(minPitch, maxPitch);
		}
	}

	public AudioData damageSound;
	public AudioData attackSound;

	void Start() {
		if (Manager.Get<SettingManager>().FxSoundEnable)
		{
			Player.OnDamageEventHandler += OnPlayerDamageEvent;
			Enemy.OnEnemyDamageEventHandler += OnEnemyDamageEvent;
		}
	}

	private void OnPlayerDamageEvent(PlayerDamageEvent e) {
		damageSound.SetPitch();
		damageSound.audioSource.Play();
	}

	private void OnEnemyDamageEvent(EnemyDamageEvent e) {
		attackSound.SetPitch();
		attackSound.audioSource.Play();
	}
}
