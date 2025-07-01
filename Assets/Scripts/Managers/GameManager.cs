using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using IA;
using IA.MVC;
using Score;
using Stamping;
using Writing;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public QuestManager questManager;
        public PaperUpdaterPosition paperUpdater;
        public PaperworkBase paperworkBase;
        
        // Spawner configuration data (each with internal object pools)
        [Header("Spawner Configuration")]
        [SerializeField] private CustomerSpawnerData _customerSpawnerData;
        [SerializeField] private SignedPaperSpawnerData _signedPaperSpawnerData;
        [SerializeField] private StampSpawnerData _stampSpawnerData;
        
        // Score management functionality
        [Header("Score Management")]
        [SerializeField] private BasicScore _tramitesCompletados = new();
        
        // Customer tracking
        private List<IAController> _activeCustomers = new List<IAController>();
        
        public static GameManager Instance { get; private set; }

        // Public access to stamp template for StampReceiver
        public GameObject StampTemplate => _stampSpawnerData?.StampTemplate;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Instance != null && Instance != this) 
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            
            // Initialize score if not already set
            if (_tramitesCompletados == null)
                _tramitesCompletados = new BasicScore();
                
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

        // Customer spawning functionality
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

        // Signed paper spawning functionality
        public void SpawnSignedPaper()
        {
            var paper = _signedPaperSpawnerData?.SpawnPaper();
            if (paper != null)
            {
                paperUpdater?.SetPaper(paper.transform);
                paperworkBase = paper.GetComponent<PaperworkBase>();
            }
        }

        public void FinishedSignedPaper(Paper paper)
        {
            _signedPaperSpawnerData?.ReturnPaper(paper);
        }

        // Stamp spawning functionality
        public void SpawnStamp()
        {
            _stampSpawnerData?.SpawnStamp();
        }

        public void FinishedStamp(StampReceiver stampReceiver)
        {
            _stampSpawnerData?.ReturnStamp(stampReceiver);
        }

        // Score management methods
        public BasicScore TramitesCompletadosScore()
        {
            return _tramitesCompletados;
        }

        // Scene management functionality
        public void ChangeScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        // Collider management
        public void StopCollider()
        {
            if (paperworkBase == null) return;
            paperworkBase.GetComponent<MeshCollider>().enabled = false;
        }

        public void StartCollider()
        {
            if (paperworkBase == null) return;
            paperworkBase.GetComponent<MeshCollider>().enabled = true;
        }
    }
}
