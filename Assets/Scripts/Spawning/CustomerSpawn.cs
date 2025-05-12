using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : ObjectSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public override void SpawnObject()
    {
        GameObject gameObject = objectPool.GetObject();
        gameObject.transform.position = this.transform.position;
    }
}
