/*
 * Class: JumpingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: Enemy spawner
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy; //enemy for spawning
    [SerializeField] private float spawnFrequency;

    private float curSpawnFrequency = 0;
    private GameObject[] enemies = new GameObject[20];
    private int idx = 0; //idx of the spawned enemy from the "enemies"

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            enemies[i] = Instantiate(enemy, transform.position, Quaternion.identity) as GameObject;
            enemies[i].transform.parent = transform;
            enemies[i].transform.position = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y, -4);
            enemies[i].SetActive(false);
        }
    }

    private void Update()
    {
        if(curSpawnFrequency < spawnFrequency)
        {
            curSpawnFrequency += Time.deltaTime;
            if(curSpawnFrequency >= spawnFrequency)
            {
                if(enemies[idx] != null)
                {
                    enemies[idx].SetActive(true);
                    int nextIdx = (idx + 1) % enemies.Length;
                    if (!enemies[nextIdx].GetComponent<Enemy>().GetIsReusable())
                    {
                        idx = nextIdx;
                    }
                    curSpawnFrequency = 0.0f;
                }
            }
        }


        //retreiving spawned enemy is they are reusable.
        for (int i = 0; i < idx; i++)
        {
            if(enemies[i].GetComponent<Enemy>().GetIsReusable())
            {
                enemies[i].transform.position = transform.position;
                enemies[i].GetComponent<Enemy>().SetIsReusable(false);
                enemies[i].SetActive(false);
            }
        }
    }
}
