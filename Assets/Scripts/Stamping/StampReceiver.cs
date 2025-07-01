using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Stamping
{
    public class StampReceiver : PaperworkBase
    {
        [SerializeField] private GameObject _childs;
        private Vector3 _lastPosition;
    
        // Add reference to XR component
        private XRGrabInteractable _xrGrabInteractable;

        private void Awake()
        {
            paperworkType = PaperworkType.stamp;
            isDone = false;
        
            // Cache reference to XR component
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        
            Debug.Log($"[StampReceiver] Awake called at position {transform.position}, XRGrabInteractable: {(_xrGrabInteractable != null ? "found" : "not found")}");
        }

        private void Start()
        {
            GameManager.Instance.questManager.onButtonPressed += Retreat;
        }

        public void InitializeStamp(Transform spawnPos)
        {
            // Set the transform position
            this.transform.position = spawnPos.position;
        
            // Ensure XRGrabInteractable is also positioned correctly
            if (_xrGrabInteractable != null)
            {
                // If XR component uses an attach transform, update it too
                if (_xrGrabInteractable.attachTransform != null)
                {
                    Debug.Log($"[StampReceiver] Setting attachTransform local position to zero");
                    _xrGrabInteractable.attachTransform.localPosition = Vector3.zero;
                }
            
                // Force refresh XR component's internal state if needed
                var rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.position = spawnPos.position;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        
            _lastPosition = spawnPos.position;
            Debug.Log($"[StampReceiver] Initialized at {this.transform.position}, XRGrabInteractable: {(_xrGrabInteractable != null ? "updated" : "not found")}");
        
            isDone = false;
            foreach (Transform child in _childs.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        private void Retreat()
        {
            if (!isDone) return;
            Debug.Log($"StampReceiver retreating at {this.transform.position}");
            GameManager.Instance.FinishedStamp(this);
        }

        public void HandleStamp(StampData data)
        {
            if (_childs == null)
            {
                Debug.LogError("Childs GameObject not assigned in StampReceiver");
                return;
            }
        
            // Get stamp template from GameManager's StampSpawnerData
            var stampTemplate = GameManager.Instance.StampTemplate;
            if (stampTemplate == null)
            {
                Debug.LogError("No stamp template found in StampSpawnerData");
                return;
            }

            // Instantiate the template from the spawner data
            GameObject stamp = Instantiate(stampTemplate, _childs.transform);
    
            // Position the stamp at the exact collision point
            var position = data.position;
            stamp.transform.position = new Vector3(position.x, position.y, position.z);


            // Get the renderer component
            Renderer stampRenderer = stamp.GetComponent<Renderer>();
            if (stampRenderer == null)
            {
                Debug.LogError("No Renderer component found on stampTemplate");
                return;
            }
    
            // Check if the material exists on the stampSO
            if (data.stampSO._stampMaterial == null)
            {
                Debug.LogError("No material found on StampSO");
                return;
            }
    
            // Get the color based on ink type
            var inkColor = GetColorFromInk(data.inkColor);

            // Create a completely new material instance
            var newMaterial = new Material(data.stampSO._stampMaterial)
            {
                color = inkColor
            };

            // Set material to the renderer
            stampRenderer.material = newMaterial;
    
            // Name the stamp with color information for easy identification
            stamp.name = $"Stamp_{data.inkColor}_{System.DateTime.Now.Ticks}";
            isDone = true;
        }

        private Color GetColorFromInk(StampInkColor ink) =>
            ink switch
            {
                StampInkColor.Red => Color.red,
                StampInkColor.Green => Color.green,
                StampInkColor.Black => Color.black,
                _ => Color.gray
            };

        private void OnEnable()
        {
            // Position might be reset by XRGrabInteractable when object is enabled from pool
            Debug.Log($"[StampReceiver] OnEnable called at position {transform.position}");

            // If position was reset to zero, apply last known valid position
            if (transform.position == Vector3.zero && _lastPosition != Vector3.zero)
            {
                Debug.Log($"[StampReceiver] Position was reset to zero, restoring to {_lastPosition}");
                transform.position = _lastPosition;
            }
        }

        private void OnDisable()
        {
            paperworkType = PaperworkType.stamp;
            isDone = false;
        }
    }
}