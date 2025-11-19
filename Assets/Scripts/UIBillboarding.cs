using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    public Camera cam;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = cam.transform.forward;
    }
}
