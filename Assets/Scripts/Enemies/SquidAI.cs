
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SquidAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public GameObject Giocatore;

    public LayerMask whatIsGround, whatIsPlayer;
    public Transform attackpoint;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [SerializeField] private bool IsThisBoss;

    private float currenttimer;
    private float Delay = 4f;
    [SerializeField] float aimMultiplier; //0.5 for small Squids 1.5 for bigger ones
    private void Start()
    {
        if (GameManager.instance.BossDestroyed[1] == true && IsThisBoss) { gameObject.SetActive(false); }   
        agent = GetComponent<NavMeshAgent>();
        Giocatore = GameObject.Find("Player");
        player = Giocatore.transform;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        float playerDistance = Vector3.Distance(transform.position, player.position);
        //Debug.Log("distanza" + playerDistance);
        if (playerDistance < 10)
        {
            currenttimer += Time.deltaTime;
            if (currenttimer >= Delay)
            {
                Giocatore.GetComponent<PlayerHealth>().TakeDamage(30f);
                currenttimer = 0f;
            }
        }

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;


    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Blocca la rotazione solo sull'asse Y (opzionale, se vuoi evitare che si inclini)
            Vector3 targetPosition = player.position + Vector3.up * aimMultiplier; // mira al centro del corpo
            Vector3 direction = (targetPosition - attackpoint.position).normalized;
            //direction.y = 0; // opzionale: ignora la differenza in altezza se vuoi sparare orizzontalmente

            // Ruota il nemico verso il giocatore (solo Y)
            transform.rotation = Quaternion.LookRotation(direction);

            // Instanzia il proiettile e spara nella direzione corretta
            Rigidbody rb = Instantiate(projectile, attackpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(direction * 50f , ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

   

}
