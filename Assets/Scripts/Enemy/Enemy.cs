using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public enum DeathReason {
		Kill, RoundClear
	}

    public int damage;
    public int health;
    public float _speed;
    public float rotateSpeed;

    public Transform playerTransform;
    public Transform rayTransform;

    new private Rigidbody2D rigidbody;

	private int playerDamage;
	private bool damageDelayCheak = true;

	private PlayerEnergy playerEnergy;
	private PlayerHealth playerHealth;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CheckFoward());
		playerDamage = Manager.Get<AbilityManager>().Get(AbilityType.Damage);

		GameObject player = GameObject.Find("Player");
		playerEnergy = player.GetComponent<PlayerEnergy>();
		playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
	{
        Rotate();

		Vector3 movePos;
        movePos = transform.right * _speed * Time.deltaTime;
        transform.position += movePos;
    }

    private void Rotate()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.Normalize();//Vector의 길이 => 무조건 1로 만듦

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed);
    }

    public void Damage(int value)
    {
        health -= value;
		if (health <= 0) {
			Death(DeathReason.Kill);
        }
    }

	public void Death(DeathReason reason)
	{
		GetComponent<Collider2D>().enabled = false;
		if (reason == DeathReason.Kill) {
			Manager.Get<RoundManager>().KillEnemy(1);
		}
		Destroy(gameObject);
	}

	public void SetStrength(float strength) { 
		damage = (int) strength * damage;
		health = (int) strength * health;;
		_speed *= strength;
		rotateSpeed *= strength;
	}

    private IEnumerator CheckFoward()
    {
        while (true)
        {
            yield return null;

            Ray2D ray = new Ray2D(rayTransform.position, transform.right);

            RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 2f);

            if (rayHit.collider == null || rayHit.collider.tag != "Enemy")
            {
                continue;
            }

            float tempSpeed = _speed;
            _speed = _speed * Random.Range(3, 7) * 0.1f;
            yield return new WaitForSeconds(0.2f);

            _speed = tempSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (damageDelayCheak == false || other.gameObject.tag != "Player")
        {
            return;
        }

		if (playerEnergy.IsRolling()) {
			Damage(playerDamage);
			StartCoroutine(DamageDelay(0.2f));
			return;
		}

		playerHealth.Damage(damage);
		StartCoroutine(DamageDelay(0.4f));
    }

    private IEnumerator DamageDelay(float delay)
    {
        damageDelayCheak = false;
        yield return new WaitForSeconds(delay);
        damageDelayCheak = true;
    }
}
