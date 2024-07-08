using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject points;
    public Collider2D spawnArea;
    public Collider2D cleanArea;
    public float spawnInterval = 2f;

    private float timer;
    [SerializeField]
    private WaitForSeconds respawnTime = new WaitForSeconds(3);
    [SerializeField]
    private StageManager stageManager;
    private void Start()
    {
       
    }

    public IEnumerator RandGroupCount(StageData data)
    {
        while(true) 
        {
            if(stageManager.spawnCount < data.spawnCount)
                RandomSpawnPoint(data);
            yield return respawnTime;
        }
    }

    public void RandomSpawnPoint(StageData data)
    {
        Vector2 RandomPosition;

        do
        {
            RandomPosition = GetRandomPositionCollider(spawnArea);
        }
        while (cleanArea.bounds.Contains(RandomPosition));

        points.transform.position = RandomPosition;
        Transform[] spawnPoints = points.GetComponentsInChildren<Transform>();


        foreach(var rcode in data.monsterIds)
        {
            int rand = UnityEngine.Random.Range(1, 6);

            for(int i = 0; i < rand; i++)
            {
                if (stageManager.spawnCount == data.spawnCount)
                    return;
                GameObject enemy = PoolManager.Instance.SpawnFromPool(rcode);
                enemy.transform.position = spawnPoints[i].position;
                stageManager.spawnCount++;
            }
        }
    }

    Vector2 GetRandomPositionCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector2(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), (UnityEngine.Random.Range(bounds.min.y, bounds.max.y)));
    }

}
