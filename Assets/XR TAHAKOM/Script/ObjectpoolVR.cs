
using System.Collections.Generic;
using UnityEngine;

public class ObjectpoolVR : MonoBehaviour
{
    public static ObjectpoolVR Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the object pool persists across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string tag, GameObject prefab, int initialSize)
    {
        if (!objectPools.ContainsKey(tag))
        {
            objectPools[tag] = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform); // Set parent to this ObjectPool object to keep the hierarchy clean
                objectPools[tag].Enqueue(obj);
            }
        }
    }

    public GameObject GetObjectFromPool(string tag)
    {
        if (objectPools.ContainsKey(tag))
        {
            if (objectPools[tag].Count > 0)
            {
                GameObject obj = objectPools[tag].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(null); // Ensure object is not parented to ObjectPool anymore
                return obj;
            }
            else
            {
                Debug.LogWarning($"Object pool for tag '{tag}' is empty.");
            }
        }
        else
        {
            Debug.LogWarning($"Object pool for tag '{tag}' does not exist.");
        }

        return null;
    }

    public void ReturnObjectToPool(string tag, GameObject obj)
    {
        if (objectPools.ContainsKey(tag))
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform); // Reparent to this ObjectPool object
            objectPools[tag].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Object pool for tag '{tag}' does not exist.");
        }
    }
}
