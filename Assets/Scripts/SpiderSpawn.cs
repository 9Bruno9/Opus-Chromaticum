using ScriptableObjects;
using UnityEngine;

public class SpiderSpawn : MonoBehaviour
{

    public GameObject boss;
    public GameObject enemy;
    public AlchemyColor[] listaColori = new AlchemyColor[3];
    private ColorManager coloremob;


    public float delay = 30f; // secondi prima di attivare la funzione
    private float timer = 0f;
    private bool hasActivated = false;
    EnemyHealth vitanemico;
    private FloatingHealthBar HealthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance.BossDestroyed[0]) { gameObject.SetActive(false); }
        
        vitanemico = enemy.GetComponent<EnemyHealth>();
        HealthBar = enemy.GetComponentInChildren<FloatingHealthBar>();
        coloremob = enemy.GetComponent<ColorManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

            if (!enemy.activeSelf)
            {
                if (hasActivated) return; // evita di attivare più volte

                timer += Time.deltaTime;

                if (timer >= (delay))
                {
                    int randomNumber = Random.Range(0, 3);
                    coloremob.changeColor(listaColori[randomNumber]);
                    enemy.transform.position = gameObject.transform.position;
                    vitanemico.currentHealth = vitanemico.maxHealth;
                    enemy.SetActive(true);
                    HealthBar.UpdateHealthBar(vitanemico.currentHealth, vitanemico.maxHealth);
                    

                    timer = 0;
                }
            }
        }


    public void eggDies()
    {
        gameObject.SetActive(false);
    }
     }


