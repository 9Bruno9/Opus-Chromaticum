using ScriptableObjects;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class BossMusic : MonoBehaviour
{
   public bool normalmusic = true;
   public GameObject Boss;
    [SerializeField] MusicManager musicManager;
    public GameObject BarrieraBoss;
    public GameObject player;
    public GameObject SpawnPlayerPosition;
    [SerializeField] private int nBoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
        
        if (GameManager.instance.BossDestroyed[nBoss] ==true) { 
            gameObject.SetActive(false);}
        player = GameObject.Find("Player");
        
    }

   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (normalmusic == true && Boss.activeSelf)
            {
                musicManager.changeSoundtrack(musicManager.bossclip);
               
                StartCoroutine(PlayBossMusicWithDelay(false));
                BarrieraBoss.SetActive(true);
                gameObject.SetActive(false);
                return;

            }
            /*else if (normalmusic == false && Boss.activeSelf)
            {
                musicManager.changeSoundtrack(musicManager.soundtrack);
                StartCoroutine(PlayBossMusicWithDelay(true));

                Debug.Log("seconda entarataa");
            }*/
        }
    }


    IEnumerator PlayBossMusicWithDelay(bool music)
    {
        //Debug.Log("Aspetto 3 secondi prima di cambiare musica...");
        yield return new WaitForSeconds(1f); // attende 3 secondi

        //musicManager.changeSoundtrack(musicManager.bossclip);
        normalmusic = music;
        Debug.Log("Boss music attivata");
    }


    public void OnBossDefeated()
    {
        BarrieraBoss.SetActive(false);
        gameObject.SetActive(false);
    }
}
