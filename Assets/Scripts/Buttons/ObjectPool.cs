using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> objectsPool;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int initialObjects;
    [SerializeField] private int maxObjects;
    [SerializeField] private int totalObjects;
    // Start is called before the first frame update
    void Awake()
    {
        objectsPool = new Queue<GameObject>();

        for (int i = 0; i < initialObjects; i++)
        {
            GameObject Object = Instantiate(objPrefab);
            Object.SetActive(false);
            objectsPool.Enqueue(Object);
        }
    }

    private void Update()
    {
        totalObjects = objectsPool.Count;
    }

    public GameObject GetObject()
    {
        if (objectsPool.Count > 0)
        {
            GameObject obj = objectsPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else if (totalObjects < maxObjects)
        {
            GameObject obj = Instantiate(objPrefab);
            totalObjects++;
            return obj;
        }
        else
        {
            // No available objects and max has been reached
            return null;
        }
    }


    public void ReturnToPool(GameObject Object)
    {
        Object.SetActive(false);
        objectsPool.Enqueue(Object);
    }

}