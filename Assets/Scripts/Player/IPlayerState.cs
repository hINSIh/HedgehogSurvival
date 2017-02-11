using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
	void StartState(Player player);

	void EndState(Player player);

	void Update(Player player);

	void OnTriggerStay(Player player, Damageable damageable);
}

public class NormalState : IPlayerState { 
	public void StartState(Player player) { }

	public void EndState(Player player) { }

	public void Update(Player player) { }

	public void OnTriggerStay(Player player, Damageable damageable) { }
}

public class RollingState : IPlayerState
{
	public void StartState(Player player) {
		player.animator.SetBool("Damage", false);
		player.animator.SetBool("Rolling", true);
	}

	public void EndState(Player player) { 
		player.animator.SetBool("Rolling", false);
	}

	public void Update(Player player) { 
		player.Energy -= player.energyData.spendSpeed * Time.deltaTime;
	}

	public void OnTriggerStay(Player player, Damageable damageable) {
		damageable.TryDamage(player);
	}
}

public class ChargeState : IPlayerState
{
	public void StartState(Player player) {
		player.animator.SetBool("Charge", true);
	}

	public void EndState(Player player) {
		player.animator.SetBool("Charge", false);
	}

	public void Update(Player player) {
		player.Energy += player.energyData.chargeSpeed * Time.deltaTime;
	}

	public void OnTriggerStay(Player player, Damageable damageable) { }

}