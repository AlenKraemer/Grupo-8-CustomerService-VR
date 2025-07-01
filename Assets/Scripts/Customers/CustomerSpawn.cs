using System.Collections.Generic;
using IA;
using IA.MVC;
using Managers;
using UnityEngine;

namespace Customers
{
    public class CustomerSpawn : MonoBehaviour
    {
        private void Start()
        {
            SpawnCustomer();
        }

        public void SpawnCustomer()
        {
            // Use GameManager's consolidated spawn method instead
            GameManager.Instance.SpawnCustomer();
        }

        public void FinishedCustomer(IAController customer)
        {
            GameManager.Instance.FinishedCustomer(customer);
        }

        public void SpawnObject()
        {
            // Use GameManager's consolidated spawn method instead
            GameManager.Instance.SpawnCustomer();
        }
    }
}
