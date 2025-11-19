using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TimidEnemy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform player;
    public Transform centrePoint;

    public float roamRange = 10f;
    public float fleeDistance = 30f;
    public float safeDistance = 100f;

    public float minPauseTime = 2f;
    public float maxPauseTime = 5f;

    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;

    private enum State { Roaming, Fleeing }
    private State currentState = State.Roaming;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        centrePoint = gameObject.transform;
    }

    void Update()
    {
        //Debug.Log("stato " + currentState);

        if (currentState == State.Fleeing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer >= safeDistance)
            {
                currentState = State.Roaming;
                agent.speed = 0f;
                StartWaiting();
            }
            else
            {
                HandleFleeing();
            }
        }

        
        switch (currentState)
        {
            case State.Roaming:
                HandleRoaming();
                break;
            case State.Fleeing:
                HandleFleeing();
                break;
        }
    }

    void HandleRoaming()
    {
        if (isWaiting)
        {
            animator.SetFloat("Speed", 0);
            agent.speed = 0f;
            waitTimer += Time.deltaTime;
            if (waitTimer >= currentWaitTime)
            {
                isWaiting = false;
                animator.SetFloat("Speed", 3.5f);
                agent.speed = 3.5f;
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

    void HandleFleeing()
    {
        //if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        //{
            
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;
            animator.SetFloat("Speed", 100);
            agent.speed = 20f;
            NavMeshHit hit;
            //if (NavMesh.SamplePosition(fleeTarget, out hit, 1f, NavMesh.AllAreas))
            //{
                
                animator.SetFloat("Speed", 20f);
                agent.speed = 20f;
                agent.SetDestination(fleeTarget);
                Debug.Log("parto");
            //}
        //}
    }

    public void ChangeToFleeing()
    {
        Debug.Log("Change To Fleeing");
        if (agent.isOnNavMesh && agent.enabled)
        {
            animator.SetFloat("Speed", 20f);
            agent.speed = 20f;
            currentState = State.Fleeing;
            isWaiting = false;
            //agent.ResetPath();
        }
    }

    void StartWaiting()
    {
        animator.SetFloat("Speed", 0);
        agent.speed = 0;
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
            agent.SetDestination(point);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        randomPoint.y = center.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}

