using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawn : ObjectSpawner
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private IAData[] customerData;
    [SerializeField] private List<IAController> customerList = new();

    private void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        IAData data = GetRandomCustomer();
        GameObject gameObject = objectPool.GetObject();
        if (gameObject == null) return;
        var customer = gameObject.GetComponent<IAController>();
        customer.Initialize(data);
        customerList.Add(customer);
        gameObject.transform.position = spawnPoint.position;
    }

    public void FinishedCustomer(IAController customer)
    {
        objectPool.ReturnToPool(customer.gameObject);
    }

    public override void SpawnObject()
    {
        GameObject gameObject = objectPool.GetObject();
        gameObject.transform.position = this.transform.position;
    }

    private IAData GetRandomCustomer()
    {
        var randomCustomer = Random.Range(0, customerData.Length);
        return customerData[randomCustomer];
    }
}
