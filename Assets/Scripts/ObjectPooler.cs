using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #region ObjectPoolerStaticInstance

    public static ObjectPooler CentralObjectPool;

    private void Awake()
    {
        CentralObjectPool = this;
    }

    #endregion

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public List<Pool> pools;

    private GameObject objectToSpawn;
    private GameObject obj;
    private GameObject allPooledObjects;

    // Start is called before the first frame update
    void Start()
    {
        allPooledObjects = new GameObject();
        allPooledObjects.name = "AllPooledObjects";

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        StartCoroutine(SpawnPool());
    }

    IEnumerator SpawnPool()
    {
        foreach (Pool currentPool in pools)
        {
            Queue<GameObject> currentObjectPool = new Queue<GameObject>();
            for (int i = 0; i < currentPool.size; i++)
            {
                obj = Instantiate(currentPool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(allPooledObjects.transform);
                currentObjectPool.Enqueue(obj);
                yield return new WaitForEndOfFrame();
            }
            poolDictionary.Add(currentPool.tag, currentObjectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }
        objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
