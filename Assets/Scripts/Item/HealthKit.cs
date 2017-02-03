using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour {

    public PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        Debug.Log("Healing!");
        playerHealth.Healing();
    }
}
