using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using IA.MVC;
using Score;
using Stamping;
using Writing;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public QuestManager QuestManager;
        public PaperUpdaterPosition PaperUpdater;
        public PaperworkBase PaperworkBase;

        [Header("Spawner Configuration")]
        [SerializeField] private CustomerSpawnerData _customerSpawnerData;
        [SerializeField] private SignedPaperSpawnerData _signedPaperSpawnerData;
        [SerializeField] private StampSpawnerData _stampSpawnerData;
        [Header("Score Management")]
        [SerializeField] private BasicScore _tramitesCompletados = new();
        
        private readonly List<IAController> _activeCustomers = new();
        public static GameManager Instance { get; private set; }
        public GameObject StampTemplate => _stampSpawnerData?.StampTemplate;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Instance != null && Instance != this) 
            {
                Destroy(this);
                return;
            }

            QuestManager ??= new QuestManager();
            
            Instance = this;
            
            // Initialize score if not already set
            _tramitesCompletados ??= new BasicScore();
                
            InitializeSpawners();
        }
        private void Start()
        {
            // Spawn initial customer
            SpawnCustomer();
        }
        private void InitializeSpawners()
        {
            // Initialize all spawner data classes with their internal pools
            _customerSpawnerData?.Initialize(transform);
            _signedPaperSpawnerData?.Initialize(transform);
            _stampSpawnerData?.Initialize(transform);
        }
        public void SpawnCustomer()
        {
            var customer = _customerSpawnerData?.SpawnCustomer();
            if (customer != null)
            {
                _activeCustomers.Add(customer);
            }
        }
        public void FinishedCustomer(IAController customer)
        {
            if (_activeCustomers.Contains(customer))
            {
                _activeCustomers.Remove(customer);
                _customerSpawnerData?.ReturnCustomer(customer);
            }
        }
        public void SpawnSignedPaper()
        {
            var paper = _signedPaperSpawnerData?.SpawnPaper();
            if (paper != null)
            {
                PaperUpdater?.SetPaper(paper.transform);
                PaperworkBase = paper.GetComponent<PaperworkBase>();
            }
        }
        public void FinishedSignedPaper(Paper paper)
        {
            _signedPaperSpawnerData?.ReturnPaper(paper);
        }
        public void SpawnStamp()
        {
            _stampSpawnerData?.SpawnStamp();
        }
        public void FinishedStamp(StampReceiver stampReceiver)
        {
            _stampSpawnerData?.ReturnStamp(stampReceiver);
        }
        public BasicScore TramitesCompletadosScore()
        {
            return _tramitesCompletados;
        }
        public void ChangeScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public void StopCollider()
        {
            if (PaperworkBase == null) return;
            PaperworkBase.GetComponent<MeshCollider>().enabled = false;
        }
        public void StartCollider()
        {
            if (PaperworkBase == null) return;
            PaperworkBase.GetComponent<MeshCollider>().enabled = true;
        }
    }
}
