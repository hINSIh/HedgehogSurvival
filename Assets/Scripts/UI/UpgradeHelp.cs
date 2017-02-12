using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHelp : MonoBehaviour {

    void Start()
    {
        this.gameObject.SetActive(false);
    }


	public void ClickQuestionMark()
    {
        this.gameObject.SetActive(true);
    }

    public void ClickButton()
    {
        this.gameObject.SetActive(false);
    }
}
