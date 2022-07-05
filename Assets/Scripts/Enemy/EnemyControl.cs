using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public EnemyStats EnemyStats { get; private set; }
    private Animator animator;

    [SerializeField]
    private GameObject weapon;

    public Image HPBar { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyStats = new EnemyStats(gameObject);
        animator = GetComponent<Animator>();
        EnemyStats.monsterDamageTaken += MonsterStats_monsterDamageTaken;
        EnemyStats.monsterHPisZero += MonsterStats_monsterHPisZero;
        navMeshAgent.speed = EnemyStats.WalkSpeed;
        weapon.GetComponent<WeaponAttack>().weaponCollision += OnWeaponCollision;
        ResetAnimVars();
        HPBar = transform.Find("Canvas").Find("HealthUI").Find("FillBar").GetComponent<Image>();
    }

    private void OnWeaponCollision(Collider other)
    {
        if (!other.CompareTag("Player") || !animator.GetBool("isAttacking") )
        {
            return;
        }

        other.gameObject.GetComponent<PlayerControl>().GetDamage(EnemyStats.Damage);
        Debug.Log($"{other.name} suffered {EnemyStats.Damage} from {name}");
    }

    void Update()
    {
        if(animator.GetBool("isAttacking") || animator.GetBool("isDead"))
        {
            navMeshAgent.speed = 0;
            return;
        }
        Move();
        AttackPlayer();
    }

    void Move()
    {
        if (navMeshAgent.speed <= 0)
            navMeshAgent.speed = EnemyStats.WalkSpeed;
        navMeshAgent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        animator.SetFloat("moveSpeed", navMeshAgent.velocity.magnitude);
    }

    public void GetDamage(float damage)
    {
        EnemyStats.TakeDamage(damage);
    }

    void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) <=
            navMeshAgent.stoppingDistance)
        {
            transform.LookAt(GameObject.FindWithTag("Player").transform);
            animator.SetTrigger("isAttacking");
        }
    }

    private void MonsterStats_monsterHPisZero(GeneralStats stats)
    {
        enabled = false;
        animator.SetBool("isDead", true);
    }

    private void MonsterStats_monsterDamageTaken(GeneralStats stats)
    {
        StartCoroutine(PlayDamageTaken());
    }

    IEnumerator PlayDamageTaken()
    {
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isDamaged", false);
    }
    
    public void ResetAnimVars()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDamaged", false);
        animator.SetBool("isDead", false);
        animator.SetFloat("moveSpeed", 0f);
    }
}