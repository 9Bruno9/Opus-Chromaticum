using ScriptableObjects;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.AI;


public class FakeEye : MonoBehaviour
{
    public GameObject projectile;
    public Transform shootPoint;
    public Transform player;

    public float fireRate = 3f;
    private float timer;
    public GameObject Camera;
    private NavMeshAgent agent;

    private bool hasExploded = false;
    public float damage = 25f;

    public GameObject sphere; 

    public void Init(Transform target, GameObject camera, AlchemyColor Colore)
    {
        player = target;
        Camera = camera;
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameObject.GetComponent<ColorManager>().ObjectColor = Colore;
        sphere.GetComponent<Renderer>().material.SetColor("_IrisColor", Colore.itemColor);
    }

    private void Update()
    {

       
    
    agent.SetDestination(player.position);
    


    transform.LookAt(player);
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            //Debug.Log("sto sparando");
            Shoot();
            timer = 0f;
        }
    }

    private void Shoot()
    {
        Vector3 dir = (player.position - shootPoint.position).normalized;
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(dir * 50f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Player"))
        {
            hasExploded = true;

            // Infliggi danno al player
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }

            // Puoi anche aggiungere effetti visivi qui
            Explode();
        }
    }

    private void Explode()
    {
        // Effetti visivi o audio opzionali
        Destroy(gameObject);
    }
}

