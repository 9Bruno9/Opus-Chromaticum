using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform player;
    public Transform centrePoint;

    public float detectionRange = 10f; // distanza per iniziare inseguimento
    public float attackRange = 5f;     // distanza per attaccare
    public float roamRange = 10f;      // raggio per movimento casuale

    public float minPauseTime = 2f;
    public float maxPauseTime = 5f;
    public float damage = 10f;

    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;

    public float attackCooldown = 0.5f;
    private float lastAttackTime = -Mathf.Infinity;
    private enum State { Roaming, Chasing, Attacking }
    private State currentState = State.Roaming;

    public float minAttackDuration = 2f;
    private bool isInAttackLock = false;
    private bool IsAttacking = false;

    public float ChasingSpeed = 14f;

    private float stuckTimer = 0f;
    private float stuckThreshold = 5f;

    [SerializeField] AudioSource AttackSound;

    void Start()
    {
        centrePoint = gameObject.transform;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        // Cambia stato in base alla distanza dal player
        if (!IsAttacking)
        {
            if (distanceToPlayer <= attackRange)
            {

                currentState = State.Attacking;
                animator.SetFloat("Speed", 0f);
                agent.speed = 0f;
            }
            else if (distanceToPlayer <= detectionRange && IsAttacking == false)
            {
                currentState = State.Chasing;
                animator.SetFloat("Speed", ChasingSpeed);
                agent.speed = ChasingSpeed;
            }
            else if (distanceToPlayer > detectionRange)
            {
                currentState = State.Roaming;
                if (isWaiting)
                {
                    animator.SetFloat("Speed", 0f);
                    agent.speed = 0f;
                }
                else
                {
                    animator.SetFloat("Speed", 3.5f);
                    agent.speed = 3.5f;
                }
            }


            switch (currentState)
            {
                case State.Roaming:
                    HandleRoaming();
                    break;
                case State.Chasing:
                    HandleChasing();
                    break;
                case State.Attacking:
                    //Debug.Log("Sto attaccando il player!");
                    HandleAttacking();
                    //StartCoroutine(LockInAttackState());
                    break;
            }
        }
    }


    // 🔹 Movimento casuale
    void HandleRoaming()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= currentWaitTime)
            {
                isWaiting = false;
                MoveToNewPoint();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartWaiting();
            animator.SetFloat("Speed", 0f);
            agent.speed = 0f;

        }
    }

    // Inseguimento del player
    void HandleChasing()
    {
        isWaiting = false;
        agent.SetDestination(player.position);
    }

    // Attacco
    void HandleAttacking()
    {
        isWaiting = false;
        IsAttacking = true;
        agent.ResetPath();
        transform.LookAt(player);
        agent.speed = 0f;
        animator.SetFloat("Speed", 0f);
        AttackSound.Play();

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // infligge danno dopo 0.5 secondi
            //StartCoroutine(LockInAttackState());
            animator.SetTrigger("Attacking");
            
            agent.speed = 0f;
            animator.SetFloat("Speed", 0f);

            StartCoroutine(DealDamageAfterDelay(attackCooldown));
            
            lastAttackTime = Time.time;
        }
        else { IsAttacking = false; }
    }

    IEnumerator DealDamageAfterDelay(float delay)
    {
        
        agent.speed = 0f;
        yield return new WaitForSeconds(delay / 2);
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            yield return new WaitForSeconds(delay / 2);
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else { yield return new WaitForSeconds(delay / 2); }
        yield return new WaitForSeconds(delay / 2);
        IsAttacking = false;
    }







    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        currentWaitTime = Random.Range(minPauseTime, maxPauseTime);
        agent.ResetPath();
        
    }

    void MoveToNewPoint()
    {
        Vector3 point;
        if (RandomPoint(centrePoint.position, roamRange, out point))
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(point);
            }
            else
            {
                // Ritenta con un nuovo punto se il path non è valido
                MoveToNewPoint();
            }
        }
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        randomPoint.y = center.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
