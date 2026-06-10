using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float delaytime;
    void Start()
    {
        Destroy(gameObject, delaytime);
    }
}
