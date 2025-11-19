using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealingItem : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float healAmount = 10; // quanto cura il player

    private PlayerHealth VitaPlayer;
    private bool MaterialCollected = false;
    private float _timeSinceLastSpawn;
    private WaitForSecondsRealtime _wait;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        VitaPlayer = player.GetComponent<PlayerHealth>();

        ActivateChildren(gameObject, true); // attiva tutti i figli all'inizio
        _wait = new WaitForSecondsRealtime(spawnInterval);
    }

    [ContextMenu("Collect")]
    public void Collect()
    {
        ActivateChildren(gameObject, false); // disattiva i figli invece che tutto
        StartCoroutine(WaitForSpawn());

    }

    [ContextMenu("Respawn")]
    public void RespawnMaterial()
    {
        ActivateChildren(gameObject, true); // riattiva i figli
        MaterialCollected = false;

    }

    private IEnumerator WaitForSpawn()
    {
        yield return _wait;
        RespawnMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && MaterialCollected == false && VitaPlayer.health != VitaPlayer.MaxHealth)
        {
            
            if (VitaPlayer != null )
            {
                VitaPlayer.GainLife(healAmount);
            }
            Collect();
            MaterialCollected = true;
        }
    }

    private void ActivateChildren(GameObject parent, bool active)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(active);
        }
    }

 
}

