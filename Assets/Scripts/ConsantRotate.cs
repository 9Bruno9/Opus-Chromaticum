using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeeds;

    private void FixedUpdate()
    {
        transform.Rotate(rotationSpeeds);
    }
}
