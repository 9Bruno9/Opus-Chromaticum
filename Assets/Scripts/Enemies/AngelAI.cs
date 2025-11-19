using ScriptableObjects;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class AngelAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public GameObject playerObject;
    public GameObject projectile;
    public GameObject barrierObject;
    

    public Transform attackPoint;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    private float maxHealth;

    public float walkPointRange;
    public float timeBetweenAttacks;
    public float minDistanceFromPlayer = 10f;

    public float triggerRange = 50f; // si attiva quando il player entra in questo raggio
    public float returnRange = 25f;  // torna alla posizione iniziale se il player si allontana oltre questo
    public GameObject fakeEyeprefab; 


    private bool alreadyAttacked;
    private bool walkPointSet;
    private Vector3 walkPoint;
    private Vector3 startingPosition;
    private EnemyHealth AngelHealth;

    public float timeBarrier;
    public float timeFakeEyes;

    private float timerFakeEyes;
    private float timerBarrier; 

    private bool barrierActive = false;
    private bool fakeEyesSpawned = false;
    private GameObject currentBarrier;

    public Transform[] fakeEyeSpawnPoints;
    private List<GameObject> fakeEyes = new List<GameObject>();


    private bool isFleeingRandomly = false;
    private float randomFleeCooldown = 2f;
    private float randomFleeTimer = 10f;

    public AlchemyColor[] colorList;
    private Renderer ColoreBarriera;
    private ColorManager AlchemyBarrier;
    private EnemyHealth vitaBarriera;


    [SerializeField] private float secondSpeed;
    [SerializeField] private float thirdSpeed;

    private bool barrierSetted = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        AngelHealth = GetComponent<EnemyHealth>();
        timerFakeEyes = 45f;
        timerBarrier = 10f;
        

    }

    private void Start()
    {
        AlchemyBarrier = barrierObject.GetComponent<ColorManager>();
        ColoreBarriera = barrierObject.GetComponent<Renderer>();
        vitaBarriera = barrierObject.GetComponent<EnemyHealth>() ;
        barrierObject.SetActive(false);
        if (GameManager.instance.BossDestroyed[2] == true)
        {
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (AngelHealth.currentHealth <= AngelHealth.maxHealth * 0.7f)
        {
            SetSpeed();
        }

            // Controllo fasi
            if (AngelHealth.currentHealth <= AngelHealth.maxHealth * 0.4f  )
        {
           
            if (timerBarrier > timeBarrier && barrierActive == false && !barrierObject.activeSelf)
            {
                ActivateBarrier();

                timerBarrier = 0f;
            }
            else
            {
                timerBarrier += Time.deltaTime;
            }
        }
        

        if (AngelHealth.currentHealth <= AngelHealth.maxHealth * 0.7f )
        {
            if (timerFakeEyes > timeFakeEyes)
            {
                SpawnFakeEyes();
                timerFakeEyes = 0f;
            }
            else
            {
                timerFakeEyes += Time.deltaTime; 
            }
        }

        // Movimento e attacco condizionati dalla distanza
        if (distance < triggerRange)
        {
            MoveMaintainingDistance();

            if (!alreadyAttacked)
                AttackPlayer();
        }
        else if (distance > returnRange)
        {
            ReturnToStartingPosition();
        }
        else
        {
            agent.SetDestination(transform.position); // resta fermo
        }
    }


    private void MoveMaintainingDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Se stiamo già scappando verso un punto casuale, aspetta il cooldown
        if (isFleeingRandomly)
        {
            randomFleeTimer -= Time.deltaTime;
            if (randomFleeTimer <= 0f)
            {
                isFleeingRandomly = false;
            }
            return; // non ricalcolare destinazioni finché dura il cooldown
        }

        if (distance < minDistanceFromPlayer)
        {
            Vector3 dir = (transform.position - player.position).normalized;
            Vector3 desiredPos = transform.position + dir * walkPointRange;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredPos, out hit, walkPointRange, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(hit.position);
                }
                else
                {
                    TriggerRandomFlee();
                }
            }
            else
            {
                TriggerRandomFlee();
            }
        }
        else
        {
            if (!walkPointSet) SearchWalkPoint();
            if (walkPointSet)
                agent.SetDestination(walkPoint);

            if (Vector3.Distance(transform.position, walkPoint) < 1f)
                walkPointSet = false;
        }
    }
    private void TriggerRandomFlee()
    {
        TryRandomWalkPoint();
        isFleeingRandomly = true;
        randomFleeTimer = randomFleeCooldown;
    }


    

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ReturnToStartingPosition()
    {
        agent.SetDestination(startingPosition);
    }

    private void AttackPlayer()
    {
        Debug.Log("ho sparato");
        transform.LookAt(player);
        Vector3 dir = (player.position - attackPoint.position).normalized;

        Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(dir * 80f, ForceMode.Impulse);

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void SpawnFakeEyes()
    {
        GameObject eye = Instantiate(fakeEyeprefab, transform.position, Quaternion.identity);
        eye.GetComponent<FakeEye>().Init(player, AngelHealth.Camera, colorList[Random.Range(0, colorList.Length)]);
        

    }

    public void ActivateBarrier()
    {
        vitaBarriera.currentHealth = vitaBarriera.maxHealth;
        
        int casuale = Random.Range(0, colorList.Length);
        Color nuovoColore = colorList[casuale].itemColor;
        nuovoColore.a = 0.5f;
        ColoreBarriera.material.color = nuovoColore;
        
        AlchemyBarrier.ObjectColor = colorList[casuale];
        barrierObject.SetActive(true);
        vitaBarriera.HealthBar.UpdateHealthBar(vitaBarriera.currentHealth, vitaBarriera.maxHealth); //in order to update the UI of barrier life
        barrierActive = true;
    }

    public void OnBarrierDestroyed()
    {
        barrierObject.SetActive(false);
        
        barrierActive = false;
    }

    
    private void SetSpeed()
    {
        if (AngelHealth.currentHealth <= AngelHealth.maxHealth * 0.7f)
        {
            agent.speed = secondSpeed;
        }
        else if (AngelHealth.currentHealth <= AngelHealth.maxHealth * 0.7f) 
        {
            agent.speed = thirdSpeed;
        }
    }

    private void TryRandomWalkPoint()
    {
        for (int i = 0; i < 10; i++) // tenta 10 volte
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
            randomDirection += transform.position;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, walkPointRange, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                return;
            }
        }

        // Se non trova nulla dopo 10 tentativi, resta fermo
        agent.SetDestination(transform.position);
    }

}

