using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject points;
    public Collider2D spawnArea;
    public Collider2D cleanArea;

    [SerializeField]
    private StageManager stageManager;
    public List<GameObject> isActivatedEnemy = new List<GameObject>();

    public void RandomSpawnPoint(string[] rcodes, int count)
    {
        Vector2 RandomPosition;

        do
        {
            RandomPosition = GetRandomPositionCollider(spawnArea);
        }
        while (cleanArea.bounds.Contains(RandomPosition));

        points.transform.position = RandomPosition;
        Transform[] spawnPoints = points.GetComponentsInChildren<Transform>();


        foreach(var rcode in rcodes)
        {
            int rand = UnityEngine.Random.Range(1, 6);

            for(int i = 0; i < rand; i++)
            {
                if (stageManager.spawnCount == count)
                    return;
                GameObject enemy = PoolManager.Instance.SpawnFromPool(rcode);
                isActivatedEnemy.Add(enemy);
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
