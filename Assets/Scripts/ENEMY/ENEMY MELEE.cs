using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public MazeGameManager gm;
    public Animator animator;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float rotationSpeed = 120f;
    public float attackDamageDelay = 0.4f;
    public AudioClip attackScream;

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private bool alreadyAttacked = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (gm == null)
            gm = FindFirstObjectByType<MazeGameManager>();

        agent.angularSpeed = 360f;
        agent.acceleration = 20f;
    }

    void Update()
    {
        if (Time.timeScale == 0 || player == null) return;

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        if (alreadyAttacked) return;

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        if (alreadyAttacked) return;

        if (animator != null) animator.SetTrigger("Attack");
        if (audioSource != null && attackScream != null) audioSource.PlayOneShot(attackScream);

        StartCoroutine(DelayedKill());
        alreadyAttacked = true;

        Invoke(nameof(ResetAttack), 1.5f);
    }

    IEnumerator DelayedKill()
    {
        yield return new WaitForSeconds(attackDamageDelay);

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange && !gm.losePanel.activeSelf)
        {
            if (gm != null) gm.PlayerCaught();
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}