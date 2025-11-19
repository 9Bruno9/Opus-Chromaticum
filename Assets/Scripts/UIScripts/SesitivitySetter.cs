using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SesitivitySetter : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private CameraRotation cameraRotation;
    private void Start()
    {

       
        
        if (!GameManager.instance.IsUnityNull() )
        {
            sensitivitySlider.value = GameManager.instance.sensitivity;
            SetSensitivity(GameManager.instance.sensitivity);
        }
        else
        {
            sensitivitySlider.value = 50;
            SetSensitivity(50);
        }
    }

    public void SetSensitivity(float newSensitivity)
    {
        if (!cameraRotation.IsUnityNull())
        {
            cameraRotation.SetSensitivity(newSensitivity);
        }
        GameManager.instance.sensitivity = newSensitivity;
    }
}