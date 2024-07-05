using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject points;
    public Collider2D spawnArea;
    public Collider2D cleanArea;
    public float spawnInterval = 2f;

    private float timer;

    //소환 테스트용(차후 지울것)
    private int spawnCount = 0;
    private Dictionary<int, string> monsterTags;

    private void Start()
    {
        monsterTags = new Dictionary<int, string>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval && spawnCount++ < 6) 
        {
            timer = 0;
            monsterTags.Clear();
            monsterTags.Add(UnityEngine.Random.Range(1, 6), "Slime");
            RandomSpawnPoint(monsterTags);
        }
    }

    public void RandomSpawnPoint(Dictionary<int, string> monsterTags)
    {
        Vector2 RandomPosition;

        do
        {
            RandomPosition = GetRandomPositionCollider(spawnArea);
        }
        while (cleanArea.bounds.Contains(RandomPosition));

        points.transform.position = RandomPosition;
        Transform[] spawnPoints = points.GetComponentsInChildren<Transform>();

        foreach(var data in monsterTags)
        {
            for(int i = 0; i < data.Key; i++)
            {
                try
                {
                    GameObject enemy = PoolManager.Instance.SpawnFromPool(data.Value);
                    enemy.transform.position = spawnPoints[i].position;
                }
                catch (IndexOutOfRangeException)
                {
                    Debug.LogError("한번에 소환 가능한 몬스터수 초과");
                }
            }
        }
    }

    Vector2 GetRandomPositionCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector2(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), (UnityEngine.Random.Range(bounds.min.y, bounds.max.y)));
    }

}
