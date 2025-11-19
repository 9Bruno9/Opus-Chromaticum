using UnityEngine;

public class Rotator : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(0.3f,0.2f,0.1f);
    }
}
