using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public string rcode;
    public SerializedMonoBehaviour prefab;
    public int size;
}

public class PoolManager : Singleton<PoolManager>
{
    public List<Pool> Pools;
    public Dictionary<string, Queue<SerializedMonoBehaviour>> PoolDictionary;
    private Transform objectPoolParent;

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

        PoolDictionary = new Dictionary<string, Queue<SerializedMonoBehaviour>>();
        objectPoolParent = new GameObject("ObjectPool").transform;
    }

    private void Start()
    {
        foreach (var pool in Pools)
        {
            CreatePool(pool);
        }

    }

    public SerializedMonoBehaviour SpawnFromPool(string rcode)
    {
        if (!PoolDictionary.ContainsKey(rcode))
        {
            Debug.LogWarning($"{rcode}를 가진 Pool이 PoolDictionary에 없습니다!");
            return null;
        }

        SerializedMonoBehaviour obj = PoolDictionary[rcode].Dequeue();
        PoolDictionary[rcode].Enqueue(obj);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public T SpawnFromPool<T>(string rcode) where T : SerializedMonoBehaviour
    {
        if (!PoolDictionary.ContainsKey(rcode))
        {
            Debug.LogWarning($"{rcode}를 가진 Pool이 PoolDictionary에 없습니다!");
            return default;
        }

        SerializedMonoBehaviour obj = PoolDictionary[rcode].Dequeue();
        PoolDictionary[rcode].Enqueue(obj);
        obj.gameObject.SetActive(true);
        return (T)obj;
    }

    private void CreatePool(Pool pool)
    {
        if (PoolDictionary.ContainsKey(pool.rcode))
        {
            Debug.LogWarning($"{pool.rcode}를 가진 pool이 이미 존재합니다!");
            return;
        }

        Queue<SerializedMonoBehaviour> objectPool = new Queue<SerializedMonoBehaviour>();
        Transform rcodeParent = new GameObject(pool.rcode).transform;
        if (pool.tag == "DamageTxt")
        {
            rcodeParent.SetParent(UIManager.Instance.canvas.transform);
        }
        else
            rcodeParent.SetParent(objectPoolParent);


        for (int i = 0; i < pool.size; i++)
        {
            SerializedMonoBehaviour obj = Instantiate(pool.prefab, rcodeParent);
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }

        PoolDictionary.Add(pool.rcode, objectPool);
    }
}
