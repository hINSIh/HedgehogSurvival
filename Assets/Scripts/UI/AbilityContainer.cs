using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AbilityContainer : MonoBehaviour, ICoinChangedListner {
	public AbilityType abilityType;

	[Header("Text UI Object")]
	public Text levelTextUI;
	public Text levelProgressTextUI;
	public Text upgradeTitleUI;
	public Text upgradeCostTextUI;

	[Header("UI Object")]
	public Slider levelProgress;
	public Button levelUpButton;

	[Header("Upgrade Disable Font Color")]
	public Color upgradeDisableFontColor;

	[Header("Upgrade Cost")]
	public int initialCapital = 1000; 
	public float increment = 1.6f;

	private AbilityManager manager;
	private int upgradeCost;
	private int level;
	private int maxLevel;

	private Color upgradeTitleTextColor;
	private Color upgradeCostTextColor;

	private const string levelText = "레벨 {0}";

	void Start() {
		Setup();
		Manager.Get<CoinManager>().AddChangedEventListener(this);
	}

	void OnDestroy() {
		Manager.Get<CoinManager>().RemoveChangedEventListener(this);
	}

	public void UpgradeClick() {
		if (Manager.Get<CoinManager>().Withdraw(upgradeCost)) {
			manager.Set(abilityType, level + 1);
			Reload();
		}
	}

	public void OnCoinChangedEvent(int value) {
		Reload();
	}

	private void Setup() { 
		manager = Manager.Get<AbilityManager>();

		maxLevel = manager.GetMaximum(abilityType);

		levelProgress.minValue = 0;
		levelProgress.maxValue = maxLevel;

		upgradeTitleTextColor = upgradeTitleUI.color;
		upgradeCostTextColor = upgradeCostTextUI.color;

		Reload();
	}

	private void Reload() {
		level = manager.Get(abilityType);
		levelProgress.value = level;

		upgradeCost = GetUpgradeCost();

		levelTextUI.text = string.Format(levelText, level);
		levelProgressTextUI.text = string.Format("{0}/{1}", level, maxLevel);
		upgradeCostTextUI.text = string.Format("{0:n0}", upgradeCost);

		bool canUpgrade = upgradeCost <= Manager.Get<CoinManager>().Coin;
		SetUpgradeActive(canUpgrade);
	}

	private void SetUpgradeActive(bool value) { 
		levelUpButton.interactable = value;
		levelUpButton.gameObject.SetActive(level != maxLevel);

		if (value)
		{
			upgradeTitleUI.text = "업그레이드";
			upgradeTitleUI.color = upgradeTitleTextColor;
			upgradeCostTextUI.color = upgradeCostTextColor;
		}
		else {
			upgradeTitleUI.text = "코인 부족";
			upgradeTitleUI.color = upgradeDisableFontColor;
			upgradeCostTextUI.color = upgradeDisableFontColor;
		}
	}

	private int GetUpgradeCost() {
		int cost = (int) (Mathf.Pow(increment, level - 1) * initialCapital);
		int remainder = cost % 100;
		return cost - remainder;
	}
}
