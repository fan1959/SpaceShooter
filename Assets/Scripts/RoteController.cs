using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoteController : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotSpeed;
    void Start()
    {
        Rigidbody rbd = GetComponent<Rigidbody>();
        rbd.angularVelocity = Random.insideUnitSphere * rotSpeed;
        
    }

    // Update is called once per frame
    
}
