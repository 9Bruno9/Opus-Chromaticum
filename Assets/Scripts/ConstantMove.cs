using UnityEngine;

public class ConstantMove : MonoBehaviour
{
    [SerializeField] private Vector3 moveSpeeds;
    [SerializeField] private Vector3 moveAmounts;
    private Vector3 _initialPosition;
    private Vector3 _newPos;
    
    
    private void Start()
    {
        _initialPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        _newPos.x = _initialPosition.x + Mathf.Sin(Time.time * moveSpeeds.x) * moveAmounts.x;
        _newPos.y = _initialPosition.y + Mathf.Sin(Time.time * moveSpeeds.y) * moveAmounts.y;
        _newPos.z = _initialPosition.z + Mathf.Sin(Time.time * moveSpeeds.z) * moveAmounts.z;
        transform.SetLocalPositionAndRotation(_newPos, transform.localRotation);
    }
}
