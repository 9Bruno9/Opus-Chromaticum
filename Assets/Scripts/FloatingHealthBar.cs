using UnityEngine;
using UnityEngine.UI; 

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera telecamera;
    [SerializeField] private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        telecamera = player.GetComponentInChildren<Camera>();
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = telecamera.transform.rotation;
    }
}
