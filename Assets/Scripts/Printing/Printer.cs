using Stamping;
using UnityEngine;

public class Printer : ObjectSpawner
{
    [SerializeField] private Transform spawnPoint;
    public override void SpawnObject()
    {
        GameObject gameObject = objectPool.GetObject();
        if (gameObject == null) return;
        gameObject.transform.position = spawnPoint.position;
        gameObject.GetComponent<StampReceiver>().InitializeStamp(spawnPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void Finished(StampReceiver stampReceiver)
    {
        objectPool.ReturnToPool(stampReceiver.gameObject);
    }
}
