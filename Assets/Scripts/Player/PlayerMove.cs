using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
	public Joystick joystick;

	[Header("Speed")]
	public float moveSpeed;
	public float transformMoveSpeed;
	public float rotateSpeed;

	[Header("Move limit option")]
	public float limitPadding;

	[Header("Script")]
	public PlayerHealth playerHealth;
	public PlayerEnergy playerEnergy;

	new private Rigidbody2D rigidbody;
	private Animator animator;
	private LimitArea limitArea;
	private Vector3 inputVector;

	private bool isTransform;

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
		if (playerEnergy.IsChargeToggle()) {
			return;
		}

		inputVector = joystick.GetInputVector();

		bool isMove = inputVector.sqrMagnitude > 0;
		animator.SetBool("Move", isMove);

		if (!isMove) {
			return;
		}

		Move();
		Rotate();
	}

	public bool IsTransform() {
		return isTransform;
	}

	public void SetTransform(bool value)
	{
		isTransform = value;
		animator.SetBool("Transform", value);
		if (value)
		{
			animator.SetBool("Damage", false);
		}
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

	private void Move()
	{
		float speed = isTransform ? transformMoveSpeed : moveSpeed;
		Vector3 movePos = transform.right * inputVector.magnitude * speed * Time.fixedDeltaTime;
		rigidbody.position = limitArea.Clamp(transform.position + movePos);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log(Time.time);
		if (animator.GetBool("Damage") || animator.GetBool("Transform") || other.gameObject.tag != "Enemy")
		{
			return;
		}

		StartCoroutine(DamageAnimation());
	}

	IEnumerator DamageAnimation() {
		animator.SetBool("Damage", true);
		yield return new WaitForSeconds(0.5f);
		animator.SetBool("Damage", false);
	}
}
