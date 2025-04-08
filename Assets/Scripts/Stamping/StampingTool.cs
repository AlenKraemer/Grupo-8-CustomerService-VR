using UnityEngine;
using UnityEngine.Events;

public class StampingTool : MonoBehaviour
{
    public Transform stampHead;
    public StampInkColor currentInkColor = StampInkColor.None;
    public StampSO currentStampSO;

    [SerializeField]
    private Renderer stampHeadRenderer; // Reference to the renderer component
    
    [SerializeField]
    private float stampForceThreshold = 0.1f; // Minimum force required to trigger a stamp
    
    [SerializeField]
    private bool autoStampOnContact = true; // Whether to automatically stamp on contact

    public StampEvent OnStamp;
    
    private bool isInContact = false;
    private StampReceiver currentReceiver = null;

    private void Awake()
    {
        // Get the renderer if not assigned
        if (stampHeadRenderer == null && stampHead != null)
        {
            stampHeadRenderer = stampHead.GetComponent<Renderer>();
        }
    }

    // Add this to the stamp head GameObject (use a child collider with isTrigger=true)
    private void OnTriggerEnter(Collider other)
    {
        // Skip collision check if there's no ink or stamp
        if (currentInkColor == StampInkColor.None || currentStampSO == null)
        {
            Debug.Log("Ignoring collision because ink color is None or no stamp is set");
            return;
        }

        var receiver = other.GetComponent<StampReceiver>();
        if (receiver != null)
        {
            Debug.Log($"Stamp head made contact with StampReceiver: {other.name}");
            currentReceiver = receiver;
            isInContact = true;

            if (autoStampOnContact)
            {
                Stamp();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var receiver = other.GetComponent<StampReceiver>();
        if (receiver != null && receiver == currentReceiver)
        {
            Debug.Log($"Stamp head left contact with StampReceiver: {other.name}");
            isInContact = false;
            currentReceiver = null;
        }
    }

    // For physics-based stamping (non-trigger approach)
    private void OnCollisionEnter(Collision collision)
    {
        // Skip collision check if there's no ink or stamp
        if (currentInkColor == StampInkColor.None || currentStampSO == null)
        {
            Debug.Log("Ignoring collision because ink color is None or no stamp is set");
            return;
        }

        var receiver = collision.gameObject.GetComponent<StampReceiver>();
        if (receiver != null && collision.relativeVelocity.magnitude > stampForceThreshold)
        {
            Debug.Log($"Stamp head collided with StampReceiver: {collision.gameObject.name} with force: {collision.relativeVelocity.magnitude}");
            currentReceiver = receiver;
            isInContact = true;

            if (autoStampOnContact)
            {
                Stamp();
            }
        }
    }

    public void SetInkColor(StampInkColor newColor)
    {
        currentInkColor = newColor;
        UpdateStampHeadColor();
    }

    private void UpdateStampHeadColor()
    {
        if (stampHeadRenderer != null)
        {
            Color inkColor = GetColorFromInk(currentInkColor);
            stampHeadRenderer.material.color = inkColor;
            Debug.Log($"Updated stamp head material color to {inkColor}");
        }
    }

    private Color GetColorFromInk(StampInkColor ink)
    {
        return ink switch
        {
            StampInkColor.Red => Color.red,
            StampInkColor.Green => Color.green,
            StampInkColor.Black => Color.black,
            _ => Color.white // Default to white for None
        };
    }

    public void SetStampSO(StampSO newStampSO)
    {
        currentStampSO = newStampSO;
    }

    public void Stamp()
    {
        if (currentStampSO == null || currentInkColor == StampInkColor.None || stampHead == null)
            return;

        var data = new StampData(
            stampHead.position,
            stampHead.rotation,
            currentStampSO,
            currentInkColor,
            stampHead.lossyScale
        );

        OnStamp?.Invoke(data);
        
        // Direct call to the receiver if in contact
        if (isInContact && currentReceiver != null)
        {
            currentReceiver.HandleStamp(data);
            Debug.Log($"Successfully stamped on {currentReceiver.name}");
        }
    }
    
    public bool IsInContactWithReceiver()
    {
        return isInContact && currentReceiver != null;
    }
}