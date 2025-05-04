using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    ObjectPool objectPool;

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void SpawnObject()
    {

        GameObject gameObject = objectPool.GetObject();
        gameObject.transform.position = this.transform.position;

    }

}
