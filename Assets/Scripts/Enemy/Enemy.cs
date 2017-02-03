using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int health;
    public float _speed;
    public float rotateSpeed;

    public PlayerHealth playerHealth;
    public Transform playerTransform;
    public Transform rayTransform;

    new private Rigidbody2D rigidbody;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CheckFoward());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePos;

        Rotate();

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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        playerHealth.Damage(damage);
    }
}
