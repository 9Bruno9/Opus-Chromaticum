using ScriptableObjects;
using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorManager : MonoBehaviour
{
    public AlchemyColor ObjectColor;
    public AlchemyColor[] ColorList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (ColorList.Length!=0)
        {
            Debug.Log("lunghezza lista" + ColorList.Length);
            ObjectColor = ColorList[Random.Range(0, ColorList.Length)];
        }


        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.SetColor("_AlteredColor", ObjectColor.itemColor);
            renderer.material.SetInt("_IsAltered", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeColor(AlchemyColor newcolor)
    {
        ObjectColor = newcolor;
        //Color.red.a  per utilizzare alfa 
        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.SetColor("_AlteredColor", ObjectColor.itemColor);
            renderer.material.SetInt("_IsAltered", 1);
        }
    }
}
