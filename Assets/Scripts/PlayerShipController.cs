using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BoundDary
{
    public float xMin, zMin, xMax, zMax;
}
public class PlayerShipController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rbd;
    public float Speed;
    public float tilt;
    public GameObject Bullet;
    public Transform shotPos;
    public BoundDary bound;

    public float shotSpace;
    private float nextShot;

    private AudioSource shotAudio;
    void Start()
    {
        rbd = GetComponent<Rigidbody>();
        shotAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton("Fire1") && nextShot < Time.time)
        {
            nextShot = Time.time + shotSpace;
            Instantiate(Bullet, shotPos.position, shotPos.rotation);
            shotAudio.Play();
            
        }
    }
    private void FixedUpdate()
    {
        //计算运动方向速度向量
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = new Vector3(h, 0, v);

        rbd.velocity = vel * Speed;

        //产生偏转
        rbd.rotation = Quaternion.Euler(0, 0, rbd.velocity.x * (-1) * tilt);

        //限制位置
        float posX = Mathf.Clamp(rbd.position.x, bound.xMin, bound.xMax);
        float posZ = Mathf.Clamp(rbd.position.z, bound.zMin, bound.zMax);
        rbd.position = new Vector3(posX, 0, posZ);
    }
}
