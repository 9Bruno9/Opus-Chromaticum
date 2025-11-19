using ScriptableObjects;
using UnityEngine;


public class ProjectileEnemy : MonoBehaviour
{
    public float lifetime = 3f;
    public int damage = 20;
    public AlchemyColor ProjectileColor;

    void Start()
    {
        Destroy(gameObject, lifetime); // autodistruzione dopo un tot di tempo
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject != null && !collision.gameObject.CompareTag("Enemy")) Destroy(gameObject);
            return;
        }
        // Controlla se il proiettile ha colpito un nemico
        
        

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null && collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }



        // In ogni caso distrugge il proiettile
        // Destroy(gameObject);
    }
}
