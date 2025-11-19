using UnityEngine;

public class SunPositioner : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;

    private void Start()
    {
        RepositionSun();
    }

    [ContextMenu("RepositionSun")]
    private void RepositionSun()
    {
        skyboxMaterial.SetVector("_SunDirection", transform.forward);
    }
}
