using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public int min_Damage = 10, max_Damage = 50;
    public int min_Health = 100, max_Health = 250;
    public int min_EnergyLeavel = 100, max_EnergyLeavel = 150;
    public float min_RotateSpeed = 1f, max_RotateSpeed = 3f;

    void Start()
    {
        PlayerPrefs.SetFloat("RotateSpeed", min_RotateSpeed);
        PlayerPrefs.SetInt("Damage", min_Damage);
        PlayerPrefs.SetInt("Health", min_Health);
        PlayerPrefs.SetInt("EnergyLeaver", min_EnergyLeavel);
    }

    public void GetRotateSpeed()
    {
        float rotateSpeedValue = PlayerPrefs.GetFloat("RotateSpeed");

        if (rotateSpeedValue < max_RotateSpeed)
        {
            PlayerPrefs.SetFloat("RotateSpeed", rotateSpeedValue + 0.2f);
        }

        rotateSpeedValue = PlayerPrefs.GetFloat("RotateSpeed");
        Debug.Log(rotateSpeedValue + "/" + max_RotateSpeed);
    }

    public void GetDamage()
    {
        int damageValue = PlayerPrefs.GetInt("Damage");

        if (damageValue < max_Damage)
        {
            PlayerPrefs.SetInt("Damage", damageValue + 10);
        }

        damageValue = PlayerPrefs.GetInt("Damage");
        Debug.Log(damageValue + "/" + max_Damage);
    }

    public void GetHealth()
    {
        int healthValue = PlayerPrefs.GetInt("Health");

        if (healthValue < max_Health)
        {
            PlayerPrefs.SetInt("Health", healthValue + 50);
        }

        healthValue = PlayerPrefs.GetInt("Health");
        Debug.Log(healthValue + "/" + max_Health);
    }

    public void GetEnergyLeavel()
    {
        int energyLeavelValue = PlayerPrefs.GetInt("EnergyLeavel");

        if (energyLeavelValue < max_EnergyLeavel)
        {
            PlayerPrefs.SetInt("EnergyLeavel", energyLeavelValue + 10);
        }

        energyLeavelValue = PlayerPrefs.GetInt("EnergyLeavel");
        Debug.Log(energyLeavelValue + "/" + max_EnergyLeavel);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("Damage", min_Damage);
        PlayerPrefs.SetInt("Health", min_Health);
        PlayerPrefs.SetInt("EnergyLeavel", min_EnergyLeavel);
        PlayerPrefs.SetFloat("RotateSpeed", min_RotateSpeed);

        Debug.Log(min_RotateSpeed + "/" + max_RotateSpeed);
        Debug.Log(min_Damage + "/" + max_Damage);
        Debug.Log(min_Health + "/" + max_Health);
        Debug.Log(min_EnergyLeavel + "/" + max_EnergyLeavel);
    }
}
