using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class Enemy : MonoBehaviour, Damageable
{
	public static event OnEnemyDeathEvent OnEnemyDeathEventHandler;
	public static event OnEnemyDamageEvent OnEnemyDamageEventHandler;

	public delegate void OnEnemyDamageEvent(EnemyDamageEvent e);
	public delegate void OnEnemyDeathEvent(EnemyDeathEvent e);

	public enum DeathReason {
		Kill, RoundClear
	}

	[Header("Death change sprite")]
	public Sprite deathSprite;

	[Header("Ability")]
    public int damage;
    public int health;
	public float moveSpeed;
    public float rotateSpeed;
	public float damageDelay;

	[Header("Gameobjet Transform")]
    public Transform playerTransform;
    public Transform rayTransform;

    new private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private bool canMove = true;
	private bool canDamage = true;
	private bool isDeath = false;
	private bool isPause = false;

	private Player player;

	public static void ClearEvents() {
		OnEnemyDeathEventHandler = delegate {};
		OnEnemyDamageEventHandler = delegate {};
	}

    // Use this for initialization
    void Start()
    {
		PauseButton pauseButton = Manager.Get<PauseButton>();
		pauseButton.OnPauseEventHandler += OnPauseEvent;
		pauseButton.OnPlayEventHandler += OnPlayEvent;

        StartCoroutine(CheckFoward());
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		player = Manager.Get<Player>();
    }

    // Update is called once per frame
    void Update()
	{
		if (isDeath || isPause)
		{
			return;
		}

		Rotate();
		if (canMove) {
			Move();
		}
    }

	void OnTriggerStay2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("Player") || isDeath)
		{
			return;
		}

		canMove = false;
		player.TryDamage(this);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canMove = true;
		}
	}

	private void Move() {
		Vector3 movePos = transform.right * moveSpeed * Time.deltaTime;
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

    private int Health
    {
		get { return health; }
		set
		{
			health = value;
			if (health <= 0)
			{
				Death(DeathReason.Kill);
			}
		}
    }

	public void Death(DeathReason reason)
	{
		if (isDeath) {
			return;
		}

		EnemyDeathEvent deathEvent = new EnemyDeathEvent(this, reason);
		OnEnemyDeathEventHandler(deathEvent);

		isDeath = true;
		Destroy(gameObject, 2);
		StartCoroutine(AnimationDeath());
	}

	private IEnumerator AnimationDeath() {
		animator.enabled = false;
		spriteRenderer.sprite = deathSprite;

		float normalize;
		for (float i = 255; i >= 0; i -= 5) {
			normalize = i / 255;
			spriteRenderer.color = new Color(normalize, normalize, normalize, normalize);
			yield return null;
		}
	}

	public void SetStrength(float strength) { 
		damage = (int) strength * damage;
		Health = (int) strength * Health;
		moveSpeed *= strength;
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

            float tempSpeed = moveSpeed;
            moveSpeed = moveSpeed * Random.Range(3, 7) * 0.1f;
            yield return new WaitForSeconds(0.2f);

            moveSpeed = tempSpeed;
        }
    }

	private void OnPauseEvent() {
		isPause = true;
	}

	private void OnPlayEvent() {
		isPause = false;
	}

	#region Damageable
	public void TryDamage(Damageable other)
	{
		if (!CanDamage())
		{
			return;
		}

		EnemyDamageEvent damageEvent = new EnemyDamageEvent(this, other.GetDamage(), Health);
		OnEnemyDamageEventHandler(damageEvent);

		Health -= other.GetDamage();
		StartCoroutine(WaitForDamageDelay());
	}

	public bool CanDamage()
	{
		return canDamage && !isDeath;
	}

	public int GetDamage()
	{
		return damage;
	}

	private IEnumerator WaitForDamageDelay()
	{
		canDamage = false;
		animator.SetBool("Damage", true);
		yield return new WaitForSeconds(damageDelay);
		animator.SetBool("Damage", false);
		canDamage = true;
	}
	#endregion
}
