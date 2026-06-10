using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform shotPos;
    //发射子弹间隙
    public float shotSpace;
    //发射子弹等待时间
    public float shotWait;

    public AudioSource shotSound;
    void Start()
    {
        InvokeRepeating("EnemyShipFire",shotWait,shotSpace);
        shotSound = GetComponent<AudioSource>();
    }
    void EnemyShipFire()
    {
        Instantiate(bullet, shotPos.position ,shotPos.rotation);
        shotSound.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
