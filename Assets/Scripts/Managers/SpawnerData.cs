using UnityEngine;
using IA;
using IA.MVC;
using Writing;
using Stamping;

namespace Managers
{
    [System.Serializable]
    public class CustomerSpawnerData
    {
        [Header("Customer Configuration")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private IAData[] _customerData;
        [SerializeField] private GameObject _customerPrefab;
        [SerializeField] private int _maxCustomers = 5;
        
        // Internal object pool - not visible in inspector
        private SerializableObjectPool _customerPool = new();

        public Transform SpawnPoint => _spawnPoint;
        public IAData[] CustomerData => _customerData;

        public void Initialize(Transform parentTransform)
        {
            if (_customerPrefab != null)
            {
                _customerPool.Initialize(_customerPrefab, _maxCustomers, parentTransform);
            }
        }

        public IAController SpawnCustomer()
        {
            IAData data = GetRandomCustomer();
            if (data == null) return null;
            
            var customer = _customerPool.GetObject()?.GetComponent<IAController>();
            if (customer != null)
            {
                customer.Initialize(data);
                customer.transform.position = _spawnPoint.position;
            }
            
            return customer;
        }

        public void ReturnCustomer(IAController customer)
        {
            _customerPool.ReturnToPool(customer.gameObject);
        }

        public IAData GetRandomCustomer()
        {
            if (_customerData == null || _customerData.Length == 0) return null;
            var randomCustomer = Random.Range(0, _customerData.Length);
            return _customerData[randomCustomer];
        }
    }

    [System.Serializable]
    public class SignedPaperSpawnerData
    {
        [Header("Paper Configuration")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _paperPrefab;
        [SerializeField] private int _maxPapers = 10;
        
        // Internal object pool - not visible in inspector
        private SerializableObjectPool _paperPool = new();

        public Transform SpawnPoint => _spawnPoint;

        public void Initialize(Transform parentTransform)
        {
            if (_paperPrefab != null)
            {
                _paperPool.Initialize(_paperPrefab, _maxPapers, parentTransform);
            }
        }

        public Paper SpawnPaper()
        {
            var paper = _paperPool.GetObject()?.GetComponent<Paper>();
            if (paper != null)
            {
                paper.transform.position = _spawnPoint.position;
                paper.InitializePaper();
            }
            
            return paper;
        }

        public void ReturnPaper(Paper paper)
        {
            _paperPool.ReturnToPool(paper.gameObject);
        }
    }

    [System.Serializable]
    public class StampSpawnerData
    {
        [Header("Stamp Configuration")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _stampPrefab;
        [SerializeField] private GameObject _stampTemplate; // Template for individual stamps
        [SerializeField] private int _maxStamps = 5;
        
        // Internal object pool - not visible in inspector
        private SerializableObjectPool _stampPool = new();

        public Transform SpawnPoint => _spawnPoint;
        public GameObject StampTemplate => _stampTemplate;

        public void Initialize(Transform parentTransform)
        {
            if (_stampPrefab != null)
            {
                _stampPool.Initialize(_stampPrefab, _maxStamps, parentTransform);
            }
        }

        public StampReceiver SpawnStamp()
        {
            var stampReceiver = _stampPool.GetObject()?.GetComponent<StampReceiver>();
            if (stampReceiver != null)
            {
                stampReceiver.transform.position = _spawnPoint.position;
                stampReceiver.InitializeStamp(_spawnPoint);
            }
            
            return stampReceiver;
        }

        public void ReturnStamp(StampReceiver stampReceiver)
        {
            _stampPool.ReturnToPool(stampReceiver.gameObject);
        }
    }
}
