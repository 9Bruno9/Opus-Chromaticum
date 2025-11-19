using UnityEngine;
using ScriptableObjects; 

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;
    public int damage = 10;
    public AlchemyColor ProjectileColor;
    
    void Start()
    {
       
        Destroy(gameObject, lifetime); // autodistruzione dopo un tot di tempo
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")){
            if (collision.gameObject != null) 
            {   
                //Debug.Log("contatto"+collision.gameObject.name);
                Destroy(gameObject); 
            }
            return;
        }
        // Controlla se il proiettile ha colpito un nemico
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        var EnemyColor = collision.gameObject.GetComponent<ColorManager>().ObjectColor;
       

        if(enemy != null) //&& EnemyColor.HitThisWith(ProjectileColor) > 0)
        {
            enemy.TakeDamage(damage* EnemyColor.HitThisWith(ProjectileColor));
            if(enemy.GetComponent<TimidEnemy>() != null)
            {
                enemy.GetComponent<TimidEnemy>().ChangeToFleeing();
            }
            Destroy(gameObject);
        }

        // In ogni caso distrugge il proiettile
        // Destroy(gameObject);
    }
}
