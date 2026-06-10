using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // Start is called before the first frame update
    public float FlySpeed;
    void Start()
    {
        Rigidbody rbd=GetComponent<Rigidbody>();
        rbd.velocity = transform.forward * FlySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
