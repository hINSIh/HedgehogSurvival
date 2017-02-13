using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : Event {
	public readonly Enemy enemy;

	public EnemyEvent(Enemy enemy) {
		this.enemy = enemy;
	}
}

public class EnemyDamageEvent : EnemyEvent, Cancellable
{
	private bool cancelled;
	private int damage;
	private int currentHealth;

	public EnemyDamageEvent(Enemy enemy, int damage, int currentHealth) : base(enemy)
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

public class EnemyDeathEvent : EnemyEvent {
	public readonly Enemy.DeathReason reason;

	public EnemyDeathEvent(Enemy enemy, Enemy.DeathReason reason) : base(enemy) {
		this.reason = reason;
	}
}
