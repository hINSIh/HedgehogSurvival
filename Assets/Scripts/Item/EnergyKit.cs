using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyKit : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

		player.Energy += player.energyData.maxEnergy / 2;
        Destroy(gameObject);
    }
}
