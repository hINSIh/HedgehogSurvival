using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damageable {
	public class StateStorage
	{
		public readonly NormalState normalState = new NormalState();
		public readonly RollingState rollingState = new RollingState();
		public readonly ChargeState chargeState = new ChargeState();
	}

	#region AbilityData
	public interface AbilityData
	{
		void SetAbilityLevel(int level);
	}

	[System.Serializable]
	public class DamageData : AbilityData
	{
		public float damageDelay = 0.33f;
		public int damage = 10;

		[Header("Upgrade")]
		public int increaseDamage = 4;

		public void SetAbilityLevel(int level)
		{
			damage += Mathf.RoundToInt(increaseDamage * level);
		}
	}

	[System.Serializable]
	public class HealthData : AbilityData {
		public int maxHealth = 100;

		[Header("Upgrade")]
		public float increaseHelath = 15;

		public void SetAbilityLevel(int level)
		{
			maxHealth += Mathf.RoundToInt(increaseHelath * level);
		}
	}

	[System.Serializable]
	public class EnergyData : AbilityData { 
		[Header("Option")]
		public float maxEnergy = 200;
		public float chargeSpeed = 70;
		public float spendSpeed = 50;

		[Header("Upgrade")]
		public float increaseEnergy = 20;
		public float increaseChargeSpeed = 15;

		public void SetAbilityLevel(int level) { 
			maxEnergy += increaseEnergy * level;
			chargeSpeed += increaseChargeSpeed * level;
		}
	}

	[System.Serializable]
	public class MoveData : AbilityData
	{
		public float moveSpeed = 5.5f;
		public float rotateSpeed = 3;

		[Header("Upgrade")]
		public float increaseMoveSpeed = 0.45f;
		public float increaseRotateSpeed = 0.4f;

		public void SetAbilityLevel(int level)
		{
			moveSpeed += increaseMoveSpeed * level;
			rotateSpeed += increaseRotateSpeed * level;
		}
	}
	#endregion

	#region Events
	public delegate void OnDamageEventHandler(PlayerDamageEvent e);
	public delegate void OnDeathEventHandler(PlayerDeathEvent e);
	public delegate void OnEnergyChangedEventHandler(EnergyChangedEvent e);
	public delegate void OnStateChangedEventHandler(PlayerStateChangedEvent e);

	public static event OnDamageEventHandler OnDamageEventListener;
	public static event OnDeathEventHandler OnDeathEventListener;
	public static event OnEnergyChangedEventHandler OnEnergyChangedEventListener;
	public static event OnStateChangedEventHandler OnStateChangedEventListener;
	#endregion

	[Header("Abilites")]
	public DamageData damageData;
	public HealthData healthData;
	public EnergyData energyData;
	public MoveData moveData;

	[Header("Player components")]
	public Animator animator;
	public PlayerMove moveScript;

	[HideInInspector]
	public readonly StateStorage stateStorage = new StateStorage();

	[SerializeField]
	private PlayerHealth healthScript;
	[SerializeField]
	private PlayerEnergy energyScript;

	private IPlayerState state;

	private bool canDamage = true;

	void Awake () {
		Manager.RegisterManager(this);
		State = stateStorage.normalState;
	}

	void Update() {
		moveScript.TryMove(this);
		state.Update(this);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		Damageable damageable = other.transform.GetComponentInParent<Damageable>();
		if (damageable != null) {
			state.OnTriggerStay(this, damageable);
		}
	}

	void OnDisable() { 
		OnDamageEventListener = delegate {};
		OnDeathEventListener = delegate {};
		OnEnergyChangedEventListener = delegate { };
		OnStateChangedEventListener = delegate { };
	}

	public int Health { 
		get { return healthScript.Health; }
		set { 
			if (value < Health) {
				int damage = Health - value;
				PlayerDamageEvent damageEvent = new PlayerDamageEvent(this, damage, healthScript.Health);
				OnDamageEventListener(damageEvent);

				if (damageEvent.IsCancelled()) {
					return;
				}
			}

			healthScript.Health = value;
			animator.SetInteger("Health", Health);

			if (Health <= 0 && enabled) {
				OnDeathEventListener(new PlayerDeathEvent(this));
			}
		}
	}

	public float Energy {
		get { return energyScript.Energy; }
		set {
			EnergyChangedEvent.ChangeType changeType;
			if (value < 0)
			{
				changeType = EnergyChangedEvent.ChangeType.Spend;
			}
			else { 
				changeType = EnergyChangedEvent.ChangeType.Charge;
			}

			float fromEnergy = energyScript.Energy;

			EnergyChangedEvent energyChangeEvent = new EnergyChangedEvent(
				this, changeType, fromEnergy - value, fromEnergy
			);

			OnEnergyChangedEventListener(energyChangeEvent);

			if (energyChangeEvent.IsCancelled()) {
				return;
			}

			energyScript.Energy = value; 
		}
	}

	public IPlayerState State { 
		get { return state; }
		set {
			if (state != null)
			{
				state.EndState(this);
			}

			if (OnStateChangedEventListener != null)
			{
				OnStateChangedEventListener(new PlayerStateChangedEvent(this, state,  value));
			}

			state = value;
			state.StartState(this);
		}
	}

	#region Damageable
	public void TryDamage(Damageable other) {
		if (!CanDamage())
		{
			return;
		}

		Health -= other.GetDamage();
		StartCoroutine(WaitForDamageDelay());
	}

	public bool CanDamage() {
		bool stateIsRolling = State == stateStorage.rollingState;
		return canDamage && !stateIsRolling;
	}

	public int GetDamage()
	{
		return damageData.damage;
	}

	private IEnumerator WaitForDamageDelay() {
		canDamage = false;
		animator.SetBool("Damage", true);
		yield return new WaitForSeconds(damageData.damageDelay);
		animator.SetBool("Damage", false);
		canDamage = true;
	}
	#endregion
}
