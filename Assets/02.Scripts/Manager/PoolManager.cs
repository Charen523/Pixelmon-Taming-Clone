using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public bool isInitActive;
    public string tag;
    public string rcode;
    public GameObject prefab;
    public int size;
}

public class PoolManager : Singleton<PoolManager>
{
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    private Transform objectPoolParent;

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        objectPoolParent = new GameObject("ObjectPool").transform;
    }

    private void Start()
    {
        foreach (var pool in Pools)
        {
            if (pool.isInitActive)
            {
                CreatePool(pool);
            }
        }
    }

    public GameObject SpawnFromPool(string rcode)
    {
        if (!PoolDictionary.ContainsKey(rcode))
        {
            Debug.LogWarning($"{rcode}를 가진 Pool이 PoolDictionary에 없습니다!");
            return null;
        }

        GameObject obj = PoolDictionary[rcode].Dequeue();
        PoolDictionary[rcode].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public void RemovePool(string rcode)
    {
        if (!PoolDictionary.ContainsKey(rcode))
        {
            Debug.LogWarning($"{rcode}를 가진 Pool이 PoolDictionary에 없습니다.");
            return;
        }

        // 해당 rcode의 부모 오브젝트 삭제
        Transform parentTransform = PoolDictionary[rcode].Peek().transform.parent;
        Destroy(parentTransform.gameObject);

        // Dictionary에서 rcode 제거
        PoolDictionary.Remove(rcode);
    }

    public void AddPool(string rcode)
    {
        Pool pool = Pools.FirstOrDefault(pool => pool.rcode == rcode);
        
        if (pool == null)
        {
            Debug.LogWarning("해당하는 Pool의 rcode가 존재하지 않습니다!");
            return;
        }

        CreatePool(pool);
    }

    private void CreatePool(Pool pool)
    {
        if (PoolDictionary.ContainsKey(pool.rcode))
        {
            Debug.LogWarning($"{pool.rcode}를 가진 pool이 이미 존재합니다!");
            return;
        }

        Queue<GameObject> objectPool = new Queue<GameObject>();
        Transform rcodeParent = new GameObject(pool.rcode).transform;
        rcodeParent.SetParent(objectPoolParent);

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = Instantiate(pool.prefab, rcodeParent);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        PoolDictionary.Add(pool.rcode, objectPool);
    }

    public void CheckInitActive(string[] rCode)
    {
        // rCode 배열을 HashSet으로 변환
        var rCodeSet = new HashSet<string>(rCode);

        // Pools에서 rcode가 rCodeSet에 있는지 확인
        foreach (var pool in Pools)
        {
            if (rCodeSet.Contains(pool.rcode))
            {
                pool.isInitActive = true;
            }
        }
    }
}
