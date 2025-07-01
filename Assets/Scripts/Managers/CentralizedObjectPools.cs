using UnityEngine;
using IA.MVC;
using Writing;
using Stamping;

namespace Managers
{
    [System.Serializable]
    public class CentralizedObjectPools
    {
        [Header("Prefab References")]
        [SerializeField] private GameObject _customerPrefab;
        [SerializeField] private GameObject _paperPrefab;
        [SerializeField] private GameObject _stampPrefab;
        
        [Header("Stamp Configuration")]
        [SerializeField] private GameObject _stampTemplate; // Template for individual stamps created by StampReceiver
        
        [Header("Pool Configuration")]
        [SerializeField] private int _maxCustomers = 5;
        [SerializeField] private int _maxPapers = 10;
        [SerializeField] private int _maxStamps = 5;
        
        [Header("Object Pools")]
        [SerializeField] private SerializableObjectPool _customerPool = new();
        [SerializeField] private SerializableObjectPool _paperPool = new();
        [SerializeField] private SerializableObjectPool _stampPool = new();

        public GameObject StampTemplate => _stampTemplate;

        public void Initialize(Transform parentTransform)
        {
            if (_customerPrefab != null)
                _customerPool.Initialize(_customerPrefab, _maxCustomers, parentTransform);
                
            if (_paperPrefab != null)
                _paperPool.Initialize(_paperPrefab, _maxPapers, parentTransform);
                
            if (_stampPrefab != null)
                _stampPool.Initialize(_stampPrefab, _maxStamps, parentTransform);
        }

        // Customer pool methods
        public IAController GetCustomer()
        {
            var customerObj = _customerPool.GetObject();
            return customerObj?.GetComponent<IAController>();
        }

        public void ReturnCustomer(IAController customer)
        {
            _customerPool.ReturnToPool(customer.gameObject);
        }

        // Paper pool methods
        public Paper GetPaper()
        {
            var paperObj = _paperPool.GetObject();
            return paperObj?.GetComponent<Paper>();
        }

        public void ReturnPaper(Paper paper)
        {
            _paperPool.ReturnToPool(paper.gameObject);
        }

        // Stamp pool methods
        public StampReceiver GetStamp()
        {
            var stampObj = _stampPool.GetObject();
            return stampObj?.GetComponent<StampReceiver>();
        }

        public void ReturnStamp(StampReceiver stamp)
        {
            _stampPool.ReturnToPool(stamp.gameObject);
        }
    }
}
