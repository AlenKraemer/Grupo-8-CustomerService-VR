using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{

    public ObjectPool objectPool;

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public abstract void SpawnObject();

}
