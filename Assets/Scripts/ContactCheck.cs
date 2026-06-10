using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyExplosion;
    public GameObject playerExplosion;
    //物体被销毁时玩家获得的分数
    public int scoreValue;

    private GameMgr gameMgr;
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameMgr");
        gameMgr=go.GetComponent<GameMgr>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }
        if (enemyExplosion != null)
        {
            Instantiate(enemyExplosion, transform.position, transform.rotation);
        }
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameMgr.GameOver();
        }
        //分数逻辑
        gameMgr.AddScore(scoreValue);

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
