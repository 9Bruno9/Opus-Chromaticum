using System;
using UnityEngine;

public class CauldronManager : MonoBehaviour
{
    [SerializeField] private GameObject incinerator;
    [SerializeField] private GameObject fountain;

    private void Start()
    {
        incinerator.SetActive(GameManager.instance.BlackAvailable);
        fountain.SetActive(GameManager.instance.WhiteAvailable);
    }
}
