using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    // Start is called before the first frame update
    public float ScrollSpeed;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Mathf.Repeat(ScrollSpeed * Time.time, 30);
        transform.position = startPos + dis * Vector3.forward *(-1);
    }
}
