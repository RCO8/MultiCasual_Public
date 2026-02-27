using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    static ObjectPools instance;
    public static ObjectPools Instance
    {
        get => instance;
        set
        {
            if (instance == null)
                instance = value;
        }
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDict;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PoolDict = new Dictionary<string, Queue<GameObject>>();
        foreach(var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject group = new GameObject(pool.tag);
            group.transform.parent = transform;
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, group.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDict.Add(pool.tag, objectPool); //PoolDict¿¡ µî·Ï
        }
    }

    public GameObject GetPrefab(string tag)
    {
        if (!PoolDict.ContainsKey(tag))
            return null;

        GameObject obj = PoolDict[tag].Dequeue();
        obj.SetActive(true);
        PoolDict[tag].Enqueue(obj);
        return obj;
    }
}
