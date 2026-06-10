using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public GameObject[] Enemys;
    //每生成一个敌人的间隔
    public float spawnWait;

    public int waveCount;

    //每波敌人之间的等待时间
    public float waveWait;
    //启动游戏后开始生成敌人的等待时间
    public float startWait;

    private int scoreCount = 0;
    //逻辑更新
    public Text lbScore;
    public GameObject endPanel;

    private bool isGameOver = false;
    void Start()
    {
        Screen.SetResolution(540,960,false);
        StartCoroutine(SpawnWaves());
    }
    //随机生成敌人物体(协程)
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < waveCount; i++)
            {
                int index = Random.Range(0, Enemys.Length);
                GameObject go = Enemys[index];
                Vector3 pos = new Vector3(Random.Range(-5, 5), 0, 12);
                Quaternion rot = Quaternion.identity;
                Instantiate(go, pos, rot);
                yield return new WaitForSeconds(spawnWait);
            }
            //跳出出生逻辑
            if (isGameOver)
            {
                break;
            }

            yield return new WaitForSeconds(waveWait);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int value)
    {
        this.scoreCount += value;
        lbScore.text = "Score:" + scoreCount.ToString();
    }

    public void GameOver()
    {
        endPanel.SetActive(true);
        isGameOver = true;
    }

    public void RestartGame()
    {
        //重新加载场景，开始新的一局游戏
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
