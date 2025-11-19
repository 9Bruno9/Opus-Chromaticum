using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public float range = 10f; // raggio di movimento
    public Transform centrePoint;

    public float minPauseTime = 2f; // tempo minimo di pausa
    public float maxPauseTime = 5f; // tempo massimo di pausa

    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        

    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
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
            //animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        currentWaitTime = Random.Range(minPauseTime, maxPauseTime);
        agent.ResetPath(); // ferma il movimento (opzionale)
    }

    void MoveToNewPoint()
    {
        Vector3 point;
        if (RandomPoint(centrePoint.position, range, out point))
        {
            agent.SetDestination(point);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        randomPoint.y = center.y; // mantieni la stessa altezza

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
