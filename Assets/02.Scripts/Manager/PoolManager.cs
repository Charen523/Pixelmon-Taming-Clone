using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public string rcode;
    public GameObject prefab;
    public int size;
}

public class PoolManager : Singleton<PoolManager>
{
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    protected override void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                if (obj.TryGetComponent(out Enemy e))
                {
                    e.data = DataManager.Instance.GetData<EnemyData>(pool.rcode);
                }
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.rcode, objectPool);
        }
    }

    public GameObject SpawnFromPool(string rcode)
    {
        if (!PoolDictionary.ContainsKey(rcode))
            return null;

        GameObject obj = PoolDictionary[rcode].Dequeue();
        PoolDictionary[rcode].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }
}
