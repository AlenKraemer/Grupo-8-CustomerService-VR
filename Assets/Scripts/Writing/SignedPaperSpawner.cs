using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignedPaperSpawner : ObjectSpawner
{
    [SerializeField] private Transform spawnPoint;
    public override void SpawnObject()
    {
        GameObject gameObject = objectPool.GetObject();
        gameObject.transform.position = spawnPoint.position;
        GameManager.Instance.paperUpdater.SetPaper(gameObject.transform);
        GameManager.Instance.paperworkBase = gameObject.GetComponent<PaperworkBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void Finished(Paper paper)
    {
        objectPool.ReturnToPool(paper.gameObject);
    }
}
