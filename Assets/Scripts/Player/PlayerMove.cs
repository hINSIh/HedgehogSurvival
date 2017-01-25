using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
	public Joystick joystick;
	[Header("Speed")]
	public float moveSpeed;
	public float rotateSpeed;
	[Header("Move limit option")]
	public float limitPadding;
	[Header("Health")]
	public PlayerHealth playerHealth;

	new private Rigidbody2D rigidbody;
	private Animator animator;
	private LimitArea limitArea;
	private Vector3 inputVector;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		SetupLimitArea();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		inputVector = joystick.GetInputVector();

		bool isMove = inputVector.sqrMagnitude > 0;
		animator.SetBool("Move", isMove);

		Move();

		if (!isMove || animator.GetBool("Damage")) {
			return;
		}

		Rotate();
	}

	private void SetupLimitArea()
	{
		limitArea = MapManager.LimitArea.AddMargin(limitPadding);
	}

	private void Rotate()
	{
		float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

		Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed);
	}

	private void Move() {
		Vector3 movePos;
		if (rigidbody.velocity.sqrMagnitude > 1) {
			movePos = Vector3.zero;
		}
		else { 
		  	movePos = transform.right * inputVector.magnitude * moveSpeed * Time.fixedDeltaTime;
		}
		rigidbody.position = limitArea.Clamp(transform.position + movePos);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (animator.GetBool("Damage") || other.gameObject.tag != "Enemy")
		{
			return;
		}

		Vector3 direction = transform.position - other.transform.position;
		direction.Normalize();

		rigidbody.velocity = direction * 4f;

		StartCoroutine(DamageAnimation());

		playerHealth.Damage(1);
	}

	IEnumerator DamageAnimation() {
		animator.SetBool("Damage", true);
		yield return new WaitForSeconds(0.5f);
		animator.SetBool("Damage", false);
	}
}
