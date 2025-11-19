using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPush : MonoBehaviour
{
    Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObj;
        if (Physics.Raycast(myTransform.position,myTransform.forward, out hitObj, 5, LayerMask.GetMask("Robot")))
        {
            Debug.Log(hitObj.rigidbody.name);
            hitObj.rigidbody.AddForce(myTransform.forward * 10 );
        }
    }
}
