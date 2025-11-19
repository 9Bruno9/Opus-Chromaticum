using System.Collections;
using NUnit.Framework;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using static Inventory;

public class Aragog : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform player;
    public Transform centrePoint;
    
    
    public float roamRange = 20f;      // raggio per movimento casuale

    public float minPauseTime = 1f;
    public float maxPauseTime = 1;
    public float damage = 10f;
    public float pushForce = 10f;
    
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private float currenttimerColor = 0f;
    [SerializeField] float ColorDelay = 15f;
    private ColorManager coloremob;


    AlchemyColor aragogColor;
    public AlchemyColor[] listaColori = new AlchemyColor[7];

    private float stuckTimer = 0f;
    private float stuckThreshold = 5f;
    private enum State { Roaming, Chasing, Attacking }
    private State currentState = State.Roaming;

    void Start()
    {
        if (GameManager.instance.BossDestroyed[0]) { gameObject.SetActive(false); }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        coloremob = GetComponent<ColorManager>();
    }

    void Update()
    {
       
        animator.SetFloat("Speed", agent.velocity.magnitude);

        currenttimerColor += Time.deltaTime;

        if (currenttimerColor >= (ColorDelay))
        {
            int randomNumber = Random.Range(0, 7);
            coloremob.changeColor(listaColori[randomNumber]);
            currenttimerColor = 0;
        }

        // Verifica se bloccato
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckThreshold)
            {
                // Forza nuovo punto
                isWaiting = false;
                MoveToNewPoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }



        HandleRoaming();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }


    // ?? Movimento casuale
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
        }
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
