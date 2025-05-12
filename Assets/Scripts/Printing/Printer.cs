using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : ObjectSpawner
{
    public override void SpawnObject()
    {
        GameObject gameObject = objectPool.GetObject();
        gameObject.transform.position = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }
}
