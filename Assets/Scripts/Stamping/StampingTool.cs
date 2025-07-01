using UnityEngine;

namespace Stamping
{
    public class StampingTool : MonoBehaviour
    {
        public Transform StampHead;
        public StampInkColor CurrentInkColor = StampInkColor.None;
        public StampSO CurrentStampSO;

        [SerializeField]
        private Renderer _stampHeadRenderer;
    
        [SerializeField]
        private float _stampForceThreshold = 0.1f;
    
        [SerializeField]
        private bool _autoStampOnContact = true;

        public StampEvent OnStamp;
    
        private bool _isInContact;
        private StampReceiver _currentReceiver;
        private Vector3 _contactPoint; // Store the contact point

        private void Awake()
        {
            if (_stampHeadRenderer == null && StampHead != null)
            {
                _stampHeadRenderer = StampHead.GetComponent<Renderer>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CurrentInkColor == StampInkColor.None || CurrentStampSO == null)
            {
                Debug.Log("Ignoring collision because ink color is None or no stamp is set");
                return;
            }

            var receiver = other.GetComponent<StampReceiver>();
            if (receiver == null) return;
            Debug.Log($"Stamp head made contact with StampReceiver: {other.name}");
            _currentReceiver = receiver;
            _isInContact = true;
            
            // Calculate the closest point on the collider to the stamp head
            _contactPoint = other.ClosestPoint(StampHead.position);
            
            if (_autoStampOnContact)
            {
                Stamp();
            }
        }
    

        private void OnCollisionEnter(Collision collision)
        {
            // First, check if this is an ink pad zone collision
            var inkPad = collision.gameObject.GetComponentInParent<InkChangerPad>();
            if (inkPad != null)
            {
                // This is a collision with an ink pad, get the ink color for this zone
                var zoneColor = inkPad.GetInkColorForCollider(collision.collider);
                if (zoneColor != StampInkColor.None)
                {
                    SetInkColor(zoneColor);
                    Debug.Log($"Changed stamp ink color to {zoneColor} from ink pad");
                    return;
                }
            }
        
            // If not an ink pad, handle normal stamp collision
            if (CurrentStampSO == null)
            {
                Debug.Log("Ignoring collision because ink color is None or no stamp is set");
                return;
            }

            var receiver = collision.gameObject.GetComponent<StampReceiver>();
        
            if (receiver == null || !(collision.relativeVelocity.magnitude > _stampForceThreshold)) return;
        
            _currentReceiver = receiver;
            _isInContact = true;
            
            // Store the contact point from the collision
            _contactPoint = collision.contacts[0].point;
            
            if (_autoStampOnContact)
            {
                Stamp();
            }
        }

        private void SetInkColor(StampInkColor newColor)
        {
            CurrentInkColor = newColor;
            // UpdateStampHeadColor() call removed - we don't want to modify the material color
            Debug.Log($"Ink color set to {newColor} without changing material");
        }

        private void Stamp()
        {
            if (CurrentStampSO == null || CurrentInkColor == StampInkColor.None || StampHead == null)
                return;

            var data = new StampData(
                _isInContact ? _contactPoint : StampHead.position, // Use contact point if in contact
                StampHead.rotation,
                CurrentStampSO,
                CurrentInkColor,
                StampHead.lossyScale
            );

            OnStamp?.Invoke(data);
        
            if (_isInContact && _currentReceiver != null)
            {
                _currentReceiver.HandleStamp(data);
            }
        }
    }
}