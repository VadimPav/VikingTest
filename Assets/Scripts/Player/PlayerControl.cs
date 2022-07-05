using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public PlayerStats stats { get; private set; }

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    Animator animator;
    Transform cameraT;

    [SerializeField] private GameObject weapon;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        stats = new PlayerStats(gameObject);

        stats.playerDamageTaken += OnDamageTaken;
        stats.playerHPisZero += Die;
        weapon.GetComponent<WeaponAttack>().weaponCollision += OnWeaponCollision;
        ResetAnimVars();
    }

    public void ResetAnimVars()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDamageTaken", false);
        animator.SetBool("isDead", false);
        animator.SetFloat("moveSpeed", 0f);
    }

    private void OnWeaponCollision(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") || !animator.GetBool("isAttacking"))
        {
            return;
        }

        other.gameObject.GetComponent<EnemyControl>().GetDamage(stats.Damage);
        Debug.Log($"{other.name} suffered {stats.Damage} from {name}");
    }

    void Update()
    {
        if (animator.GetBool("isDead"))
        {
            return;
        }
        if (Input.GetButton("Fire1"))
        {
            StartCoroutine(Attack());
        }

        Move();
    }

    private void Die(GeneralStats stats)
    {
        animator.SetBool("isDead", true);
    }

    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isAttacking", false);
    }

    void OnDamageTaken(GeneralStats stats)
    {
        StartCoroutine(PlayDamageAnim());
    }

    IEnumerator PlayDamageAnim()
    {
        animator.SetTrigger("isDamageTaken");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                        ref turnSmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = stats.WalkSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        animator.SetFloat("moveSpeed", inputDir == Vector2.zero ? 0 : 0.5f);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }

    public void GetDamage(float damage)
    {
        stats.TakeDamage(damage);
    }

    public void Heal(float healAmount)
    {
        stats.Heal(healAmount);
    }
}