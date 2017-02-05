using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int maxHealth;
	public Slider healthSlider;

	[Header("Status UI")]
	public Image statusImage;
	public Sprite[] statusSprites;

	private int currentHealth;
	private int currentStatus;

	void Start () {
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		currentHealth = maxHealth;
		SetStatusImage();
	}

	public void Damage(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			GameOver();
			currentHealth = 0;
		}

		SetStatusImage();
		healthSlider.value = currentHealth;
	}

    public void Healing()
    {
        currentHealth = maxHealth;
    }

	private void GameOver() {
		Debug.Log("GameOver!");
		gameObject.SetActive(false);
		// TODO gameover
	}

	private void SetStatusImage() {
		float unit = (float) maxHealth / statusSprites.Length;
		int result = 0;

		for (int i = 1; i < statusSprites.Length; i++) {
			if (currentHealth >= unit * i) {
				continue;
			}

			result = statusSprites.Length - i;
			break;
		}

		if (currentStatus == (int) result) {
			return;
		}

		currentStatus = result;
		statusImage.sprite = statusSprites[result];
	}
}
