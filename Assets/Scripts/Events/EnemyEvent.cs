using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : Event {
	public readonly Enemy enemy;

	public EnemyEvent(Enemy enemy) {
		this.enemy = enemy;
	}
}

public class EnemyDeathEvent : EnemyEvent {
	public readonly Enemy.DeathReason reason;

	public EnemyDeathEvent(Enemy enemy, Enemy.DeathReason reason) : base(enemy) {
		this.reason = reason;
	}
}
