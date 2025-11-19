using ScriptableObjects;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool alreadyDestroyed;
    [SerializeField] int numberObstacle;
    public AlchemyColor alchemyColor;

    private void Start()
    {
        if (GameManager.instance.obstacleDestroyed[numberObstacle]) { gameObject.SetActive(false); }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<Projectile>().ProjectileColor == alchemyColor)
            {
                gameObject.SetActive(false);
                GameManager.instance.obstacleDestroyed[numberObstacle] = true;  
            }
            return;
        }
        
    }

        
}
