using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private StageManager stageManager;
    private Camera cam;

    public GameObject points;
    [SerializeField] private Collider2D spawnArea;
    [SerializeField]private Collider2D cleanArea;

    public List<Enemy> isActivatedEnemy = new List<Enemy>();
    public GameObject[] dungeonBoss;

    private void Awake()
    {
        stageManager = StageManager.Instance;   
        cam = Camera.main;
    }

    public async void SpawnMonsterTroop(string[] rcodes, int totalCount)
    {
        if (stageManager.curSpawnCount >= totalCount) return;

        //이번에 스폰될 몬스터의 종류
        int randEnemyType = Random.Range(0, rcodes.Length);
        EnemyData curEnemy = DataManager.Instance.GetData<EnemyData>($"{rcodes[randEnemyType]}");

        //이번에 스폰될 몬스터의 마리 수
        int maxSpawnNum = Mathf.Min(totalCount - stageManager.curSpawnCount + 1, 6);
        int randSpawnCount = Random.Range(1, maxSpawnNum);
        
        //이번에 스폰될 몬스터의 위치
        Vector2 RandomPos;
        do
        {
            RandomPos = GetRandomPos(spawnArea);
        }
        while (cleanArea.bounds.Contains(RandomPos));

        points.transform.position = RandomPos;
        Transform[] spawnPoints = points.GetComponentsInChildren<Transform>();


        //실제 소환
        for (int i = 0; i < randSpawnCount; i++)
        {
            if (i >= spawnPoints.Length) break;

            Enemy enemy = PoolManager.Instance.SpawnFromPool<Enemy>("Enemy");
            enemy.transform.position = spawnPoints[i].position;
            enemy.fsm.anim.runtimeAnimatorController = await ResourceManager.Instance.LoadAsset<RuntimeAnimatorController>(curEnemy.rcode, eAddressableType.animator);
            enemy.statHandler.data = curEnemy;
            enemy.statHandler.UpdateEnemyStats();
            isActivatedEnemy.Add(enemy);

            if (enemy.statHandler.data.isBoss)
            {
                enemy.bossHealthSystem.InvokeBossHp();

            }
            stageManager.curSpawnCount++;
        }
    }

    private Vector2 GetRandomPos(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector2(Random.Range(bounds.min.x, bounds.max.x), (Random.Range(bounds.min.y, bounds.max.y)));
    }

    public GameObject GetDgMonster(int index)
    {
        GameObject boss = Instantiate(dungeonBoss[index]);
        return boss;
    }
}
