using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeController : MonoBehaviour
{
    //闪避最大最小速度
    public float dodgeMinSpeed;
    public float dodgeMaxSpeed;
    //随机闪避目标速度
    private float dodgeTargetSpeed;
    //开启第一次闪避等待的最小最大时间
    public float waitMax;
    public float waitMin;
    //闪避最短最长时间
    public float dodgeMinTime;
    public float dodgeMaxTime;

    private Rigidbody rbd;
    //加速度
    public float accelerSpeed;

    public float tilt;

    public BoundDary bound;
    void Start()
    {
        rbd= GetComponent<Rigidbody>();
        StartCoroutine(CalcDodgeSpeed());   
    }

    IEnumerator CalcDodgeSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
            dodgeTargetSpeed = Random.Range(dodgeMinSpeed, dodgeMaxSpeed);
            if (transform.position.x > 0)
            {
                dodgeTargetSpeed = -dodgeTargetSpeed;
            }
            yield return new WaitForSeconds(Random.Range(dodgeMinTime, dodgeMaxTime));
            dodgeTargetSpeed = 0;
        }
    }
    private void FixedUpdate()
    {
        float dodgeVal = Mathf.MoveTowards(rbd.velocity.x,dodgeTargetSpeed,Time.deltaTime*accelerSpeed);
        rbd.velocity = new Vector3(dodgeVal,0,rbd.velocity.z);
        //产生偏转
        rbd.rotation = Quaternion.Euler(0, 0, rbd.velocity.x * (-1) * tilt);

        //限制位置
        float posX = Mathf.Clamp(rbd.position.x, bound.xMin, bound.xMax);
        float posZ = Mathf.Clamp(rbd.position.z, bound.zMin, bound.zMax);
        rbd.position = new Vector3(posX, 0, posZ);
    }
}
