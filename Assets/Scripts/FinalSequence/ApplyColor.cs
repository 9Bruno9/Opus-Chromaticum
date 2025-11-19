using ScriptableObjects;
using UnityEngine;

public class ApplyColor : MonoBehaviour
{
    [SerializeField] private AlchemyColor alchemyColor;
    
    private void Start()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", alchemyColor.itemColor);
    }
}
