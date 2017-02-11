using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable {
	void TryDamage(Damageable other);

	bool CanDamage();

	int GetDamage();
}
