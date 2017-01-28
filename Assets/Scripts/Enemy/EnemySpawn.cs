using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//inspecter뷰에서 수정할 수 없음
public class Round
{
    public int monsterCount;
    public float spawnDelay;
    public Enemy[] enemys;
}

public class EnemySpawn : MonoBehaviour
{

    public Transform player;

    public Round[] rounds;
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        for (int round = 0; round < rounds.Length; round++)
        {
            int randomPoint = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomPoint];

            Round currentRound = rounds[round];
            WaitForSeconds spawnDelay = new WaitForSeconds(currentRound.spawnDelay);

            for (int count = 0; count < currentRound.monsterCount; count++)
            {
                int randomMonb = Random.Range(0, currentRound.enemys.Length);
                Enemy enemy = currentRound.enemys[randomMonb];

                GameObject enemyObject = Instantiate(enemy.gameObject);
                enemyObject.transform.position = spawnPoint.position;

                enemyObject.GetComponent<Enemy>().playerTransform = player;

                enemyObject.name = "Enemy_" + (round + 1) + "R";
                yield return spawnDelay;
            }
        }
    }
}

