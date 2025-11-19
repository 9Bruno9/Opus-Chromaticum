using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Crytsal : MonoBehaviour
{
    public AlchemyColor alchemyColor;
    [SerializeField] private GameObject colorMaterial;
    public int materialGainMin;
    public int materialGainMax;
    private Renderer[] r;

    private Inventory Inventario;
    bool MaterialCollected = false;
    [SerializeField] AudioClip Sound;


    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        Inventario = player.GetComponent<Inventory>();



        if (materialGainMin > materialGainMax)
        {
            Debug.LogWarning("Ranges for material is zero or negative, values clamped. Please check your values");
            materialGainMin = materialGainMax;
        }

        r = colorMaterial.GetComponentsInChildren<Renderer>();
        
        foreach (var render in r)
        {
            render.material.SetColor("_BaseColor", alchemyColor.itemColor);
        }
        


    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player") )
        {
            MusicManager.instance.riproduceSoundEffect(Sound);
            Inventario.AddMaterial(alchemyColor, Random.Range(materialGainMin, materialGainMax));
            Destroy(gameObject);
        }
    }





}