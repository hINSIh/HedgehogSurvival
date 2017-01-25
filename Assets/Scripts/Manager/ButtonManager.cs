using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    AbilityManager ab = new AbilityManager();

    public void ButtonClick(string str)
    {
        switch (str)
        {
            case "RotateSpeed": 
                ab.GetRotateSpeed();
                break;
            case "Damage": 
                ab.GetDamage();
                break;
            case "Health":
                ab.GetHealth();
                break;
            case "EnergyLeavel":
                ab.GetEnergyLeavel();
                break;
            case "Reset":
                ab.Reset();
                break;
        }
    }
}
