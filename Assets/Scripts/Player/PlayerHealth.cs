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
	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();

		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		currentHealth = maxHealth;
		SetStatusImage();
	}

	public void Damage(int damage) {
		Health -= damage;
	}

    public void Healing()
    {
        Health = maxHealth;
    }

	private int Health { 
		get { return currentHealth; }
		set {
			currentHealth = value;
			healthSlider.value = value;
			SetStatusImage();

			if (currentHealth <= 0) {
				GameOver();
				currentHealth = 0;
			}

			animator.SetInteger("Health", currentHealth);
		}
	}

	private void GameOver() {
		StartCoroutine(Manager.Get<RoundManager>().GameOver());

		GetComponent<PlayerMove>().enabled = false;
		GetComponent<PlayerEnergy>().enabled = false;
		enabled = false;
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
