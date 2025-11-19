using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MaterialSpawner : MonoBehaviour
{
    [SerializeField] private List<AlchemyColor> possibleColors;
    [SerializeField] private AlchemyColor alchemyColor;
    [SerializeField] private GameObject colorMaterial;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int materialGainMin;
    [SerializeField] private int materialGainMax;
    private Renderer[] r;
    [SerializeField] AudioSource Sound;


    private Inventory Inventario;
    bool MaterialCollected = false;
    


    private float _timeSinceLastSpawn;
    private WaitForSecondsRealtime _wait;
    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        Inventario = player.GetComponent<Inventory>();
        


        if (materialGainMin > materialGainMax)
        {
            Debug.LogWarning("Ranges for material is zero or negative, values clamped. Please check your values");
            materialGainMin = materialGainMax;
        }
        
        colorMaterial.SetActive(true);

        alchemyColor = possibleColors[Random.Range(0,possibleColors.Count)];
        
        r = colorMaterial.GetComponentsInChildren<Renderer>();
        Debug.Log(r);
        foreach (var render in r)
        {
            render.material.SetColor("_BaseColor", alchemyColor.itemColor);
        }
        _wait = new WaitForSecondsRealtime(spawnInterval);
        
    }

    [ContextMenu("Collect")]
    public int Collect()
    {
        colorMaterial.SetActive(false);
        alchemyColor = possibleColors[Random.Range(0,possibleColors.Count)];
        StartCoroutine(WaitForSpawn());
        
        return Random.Range(materialGainMin, materialGainMax);
    }
    
    [ContextMenu("Respawn")]
    public void RespawnMaterial()
    {
        colorMaterial.SetActive(true);
        MaterialCollected = false;

        alchemyColor = possibleColors[Random.Range(0, possibleColors.Count)];

        
        Debug.Log(r);
        foreach (var render in r)
        {
            render.material.SetColor("_BaseColor", alchemyColor.itemColor);
        }
    }

    private IEnumerator WaitForSpawn()
    {
        yield return _wait;
        RespawnMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && colorMaterial && MaterialCollected== false )
        {

            Sound.Play();
            Inventario.AddMaterial(alchemyColor, Collect());
            
            MaterialCollected = true;
           
        }
    }


   

    
}
