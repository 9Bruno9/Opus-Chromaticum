using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health ;
    public float MaxHealth ;
    public Image healthBar;
    public bool IsTransport = false;
    public GameObject deathPanel;
    public GameObject generalPanel;
    public GameObject pieMenu;

    [SerializeField] private AudioSource hurtSoundSource;


    /*void OnDestroy()
    {
        // Salvo la salute residua prima di distruggere il player
        if (GameManager.instance != null && )
        {
            GameManager.instance.savedHealth = health;
            GameManager.instance.savedMaxHealth = MaxHealth;
        }
    } */


    void Start()
    {
        
        // Se i dati sono gi� salvati nel GameManager, li uso
        if ( GameManager.instance.savedHealth > 0)
        {
            health = GameManager.instance.savedHealth;
            MaxHealth = GameManager.instance.savedMaxHealth;
            
        }
        else
        {
            health = MaxHealth;
            GameManager.instance.savedHealth = health;
            GameManager.instance.savedMaxHealth = MaxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        GameManager.instance.savedHealth = health;
        Debug.Log($"{gameObject.name} ha subito {amount} danni. HP rimasti: {health}");
        hurtSoundSource.Play();
        if (health <= 0)
        {
            Die();
        }
    }

    public void GainLife(float amount)
    {
        health += amount;
        if (health >= MaxHealth)
        {
            health = MaxHealth;
        }
        GameManager.instance.savedHealth = health;
        Debug.Log($"{gameObject.name} ha recuperato {amount} di vita. HP rimasti: {health}");

       
    }


    private void Die()
    {
        GameManager.instance.isAMenuOpen = true;
        pieMenu.SetActive(false);
        generalPanel.SetActive(false);
        deathPanel.SetActive(true);
    }

    
    void Update()
    {
        if (healthBar != null)
        { 
        healthBar.fillAmount = Mathf.Clamp01(health / MaxHealth);
        healthBar.fillAmount = Mathf.Clamp01(health / MaxHealth);
        }
    }
}

