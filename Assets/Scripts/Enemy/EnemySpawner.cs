/*
 * Class: JumpingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: 런타임에서 계속 스폰되는 적을 다루는 클래스
*/

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy; //스폰 할 적
    [SerializeField] private float spawnFrequency;

    private float curSpawnFrequency = 0; //스폰 주기 체크용 타이머 변수
    private const int arrLength = 20;
    private GameObject[] enemies = new GameObject[arrLength]; 
    private int idx = 0; //스폰되는 적의 인덱스 번호

    private void Start()
    {
        //게임 실행시 미리 전부 생성 후 끄기
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
        //주기마다 적의 상태를 SetActive(true)로 변경
        if(curSpawnFrequency < spawnFrequency)
        {
            curSpawnFrequency += Time.deltaTime;
            if(curSpawnFrequency >= spawnFrequency)
            {
                if(enemies[idx] != null)
                {
                    //인덱스 끝에 도달하면 다시 0부터 시작하며 인덱스가 참조하는 적이 사용 불가능이면 다음 주기까지 기다림
                    //만약 많이 기다려도 적이 안나오면 arrLength의 값을 증가.
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


        //어레이를 돌며 재활용 가능해진 적이 있는지 체크
        for (int i = 0; i < idx; i++)
        {
            if(enemies[i].GetComponent<Enemy>().GetIsReusable())
            {   
                enemies[i].transform.position = new Vector3(transform.position.x, transform.position.y, -4);
                enemies[i].GetComponent<Enemy>().SetIsReusable(false);
                enemies[i].SetActive(false);
            }
        }
    }
}
