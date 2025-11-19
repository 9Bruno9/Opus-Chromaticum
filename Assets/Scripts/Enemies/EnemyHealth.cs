using ScriptableObjects;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;
    public FloatingHealthBar HealthBar;
    public GameObject Camera;
    public GameObject cristal;
    [SerializeField] bool isBoss;
    [SerializeField] int numberBoss;

    [SerializeField] AlchemyColor black;
    

    public GameEvent BossDie;
    public GameEvent Evento;
    void Start()
    {
        currentHealth = maxHealth;
        HealthBar = GetComponentInChildren<FloatingHealthBar>();
        GameObject player = GameObject.Find("Player");
        Camera = player.GetComponentInChildren<Camera>()?.gameObject;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
        // Ottieni la direzione in cui guarda la telecamera
        Vector3 cameraForward = Camera.transform.forward;

        // Calcola una posizione davanti al nemico verso la camera
        Vector3 popupPosition = Camera.transform.position + cameraForward * 6 + new Vector3(0, 0f, 0); // altezza e distanza regolabili


        DamagePopUpGenerator.current.CreatePopUp(popupPosition + randomness, amount.ToString(), Color.yellow);
        //DamagePopUpGenerator.current.CreatePopUp(playerPos.transform.position + offset + randomness, amount.ToString(), Color.yellow);
        //DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, amount.ToString(), Color.yellow);
        HealthBar.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} ha subito {amount} danni. HP rimasti: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} è stato distrutto!");

        if (isBoss == true)
        {
          
            GameManager.instance.BossDestroyed[numberBoss] = true;
            
            MusicManager.instance.changeSoundtrack(MusicManager.instance.soundtrack);
            BossDie.TriggerEvent();
        }

        if (Evento != null)
        {
            Evento.TriggerEvent();
        }



        if (cristal != null)
        {
            GameObject cristallo = Instantiate(cristal, gameObject.transform.position, Quaternion.identity);
            AlchemyColor coloreMob = gameObject.GetComponent<ColorManager>().ObjectColor;
            Crytsal cristalStat = cristallo.GetComponent<Crytsal>();
            if (coloreMob.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Primary)
            {
                cristalStat.alchemyColor = ColorMaster.Instance.GetPrimaryColors(coloreMob)[Random.Range(0,2)];
                cristalStat.materialGainMin = (int)maxHealth / 10 ;
                cristalStat.materialGainMax = (int)maxHealth / 10 +10 ;
            }
            else if (coloreMob.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Secondary)
                {

                cristalStat.alchemyColor = ColorMaster.Instance.GetSecondaryColors(coloreMob)[Random.Range(0, 2)];
                cristalStat.materialGainMin = (int)maxHealth / 10;
                cristalStat.materialGainMax = (int)maxHealth / 10 + 10;
                }

            else if (coloreMob==black) 
            {
                cristalStat.alchemyColor = black;
                cristalStat.materialGainMin = (int)maxHealth / 10;
                cristalStat.materialGainMax = (int)maxHealth / 10 + 10;
            }
            else if (coloreMob.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Complex)
            {
                cristalStat.alchemyColor = ColorMaster.Instance.GetComplexColors()[Random.Range(0, 2)];
                cristalStat.materialGainMin = (int)maxHealth / 10;
                cristalStat.materialGainMax = (int)maxHealth / 10 + 10;
            }
          
        }

        gameObject.SetActive(false);
    }
}
