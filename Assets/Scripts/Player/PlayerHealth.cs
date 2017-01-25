using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int maxHealth;
	public Slider healthSlider;

	private int currentHealth;

	void Start () {
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		currentHealth = maxHealth;
	}
	
	void Update () {
		
	}

	public void Damage(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			GameOver();
			currentHealth = 0;
		}

		healthSlider.value = currentHealth;
	}

	private void GameOver() {
		Debug.Log("GameOver!");
		gameObject.SetActive(false);
		// TODO gameover
	}
}
