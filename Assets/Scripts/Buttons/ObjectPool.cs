using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    Queue<GameObject> objectsPool;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int MaxObjects;
    // Start is called before the first frame update
    void Awake()
    {
        objectsPool = new Queue<GameObject>();

        for (int i = 0; i < MaxObjects; i++)
        {
            GameObject Object = Instantiate(objPrefab);
            Object.SetActive(false);
            objectsPool.Enqueue(Object);
        }
    }


    public GameObject GetObject()
    {
        if (objectsPool.Count > 0)
        {
            GameObject Object = objectsPool.Dequeue();
            Object.SetActive(true);
            return Object;
        }

        else
        {
            GameObject Object = Instantiate(objPrefab);
            return Object;

        }
    }


    public void ReturnToPool(GameObject Object)
    {
        Object.SetActive(false);
        objectsPool.Enqueue(Object);
    }

}