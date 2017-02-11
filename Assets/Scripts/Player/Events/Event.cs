using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
}

public abstract class CancellableEvent : Cancellable
{
	private bool cancelled;

	public void SetCancelled(bool cancel)
	{
		cancelled = cancel;
	}

	public bool IsCancelled()
	{
		return cancelled;
	}
}

public interface Cancellable
{
	void SetCancelled(bool cancel);

	bool IsCancelled();
}

public class PlayerEvent {
	private Player player;

	public PlayerEvent(Player player) {
		this.player = player;
	}

	public Player GetPlayer()
	{
		return player;
	}
}

public class DamageEvent : PlayerEvent, Cancellable
{
	private bool cancelled;
	private int damage;
	private int currentHealth;

	public DamageEvent(Player player, int damage, int currentHealth) : base(player)
	{
		this.damage = damage;
		this.currentHealth = currentHealth;
	}

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}

	public int CurrentHealth
	{
		get { return currentHealth; }
	}

	public void SetCancelled(bool cancel)
	{
		cancelled = cancel;
	}

	public bool IsCancelled()
	{
		return cancelled;
	}
}

public class EnergyChangedEvent : PlayerEvent, Cancellable
{
	public enum ChangeType { 
		Spend, Charge
	}

	public readonly ChangeType changeType;

	public readonly float fromEnergy;
	public readonly float toEnergy;

	private bool cancelled;

	private float changeValue;

	public EnergyChangedEvent(Player player, ChangeType changeType,
	                         float changeValue, float fromEnergy) : base(player) {
		this.changeType = changeType;
		this.changeValue = changeValue;
		this.fromEnergy = fromEnergy;

		if (changeType == ChangeType.Charge)
		{
			toEnergy = fromEnergy + changeValue;
		}
		else { 
			toEnergy = fromEnergy - changeValue;
		}

		toEnergy = Mathf.Clamp(toEnergy, 0, player.energyData.maxEnergy);
	}

	public float ChargeValue
	{
		get { return changeValue; }
		set { changeValue = value; }
	}

	public void SetCancelled(bool cancel)
	{
		cancelled = cancel;
	}

	public bool IsCancelled()
	{
		return cancelled;
	}
}

public class StateChangedEvent : PlayerEvent {
	public readonly IPlayerState fromState;
	public readonly IPlayerState changedState;

	public StateChangedEvent(Player player, IPlayerState fromState, IPlayerState changedState) : base(player) {
		this.fromState = fromState;
		this.changedState = changedState;
	}
}

public class DeathEvent : PlayerEvent
{
	public DeathEvent(Player player) : base(player) { }
}