using System.Collections.Generic;
using UnityEngine;

public class PuzzleActivator : MonoBehaviour
{
    [SerializeField] private List<Transform> objectsToMove;

    [SerializeField] private Vector3 targetPosition;
    
    
    public void ActivatePuzzle()
    {
        foreach (var obj in objectsToMove)
        {
            obj.SetPositionAndRotation(new Vector3(obj.position.x, 0, obj.position.z), obj.rotation);
        }
    }

    public void ClosePuzzle()
    {
        foreach (var obj in objectsToMove)
        {
            obj.SetPositionAndRotation(new Vector3(obj.position.x, -10, obj.position.z), obj.rotation);
        }
    }
    
}
