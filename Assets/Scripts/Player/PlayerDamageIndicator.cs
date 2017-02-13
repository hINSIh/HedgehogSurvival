using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageIndicator : MonoBehaviour {

    [Header("text1")]
    public Transform text1;
    public Animator textAnimator1;
    [Header("text2")]
    public Transform text2;
    public Animator textAnimator2;

    private bool checkText;

    // Use this for initialization
    void Start () {
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        Player.OnDamageEventHandler += OnPlayerDamageEvent;
    }

    public void OnPlayerDamageEvent(PlayerDamageEvent e)
    {
        StartCoroutine(DamageIndicator(e));
    }

    IEnumerator DamageIndicator(PlayerDamageEvent e)
    {
        int damage = e.Damage;
        if (checkText)
        {
            text1.gameObject.SetActive(true);
            text1.GetComponent<Text>().text = "-" + damage;
            //textAnimator1.SetBool("Move", true);
            //textAnimator1.SetBool("Move", false);
            yield return new WaitForSeconds(0.1f);
            text1.gameObject.SetActive(false);

            checkText = true;
        }
        else
        {
            text2.gameObject.SetActive(true);
            text2.GetComponent<Text>().text = "-" + damage;
            //textAnimator2.SetBool("Move", true);
            //textAnimator2.SetBool("Move", false);
            yield return new WaitForSeconds(0.1f);
            text2.gameObject.SetActive(false);
        }
    }
}
