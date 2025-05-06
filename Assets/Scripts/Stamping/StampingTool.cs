using UnityEngine;
using UnityEngine.Events;

public class StampingTool : MonoBehaviour
{
    public Transform stampHead;
    public StampInkColor currentInkColor = StampInkColor.None;
    public StampSO currentStampSO;

    [SerializeField]
    private Renderer stampHeadRenderer;
    
    [SerializeField]
    private float stampForceThreshold = 0.1f;
    
    [SerializeField]
    private bool autoStampOnContact = true;

    public StampEvent OnStamp;
    
    private bool isInContact = false;
    private StampReceiver currentReceiver = null;
    private Vector3 contactPoint; // Store the contact point

    private void Awake()
    {
        if (stampHeadRenderer == null && stampHead != null)
        {
            stampHeadRenderer = stampHead.GetComponent<Renderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
            
            // Calculate closest point on the collider to the stamp head
            contactPoint = other.ClosestPoint(stampHead.position);
            
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

    private void OnCollisionEnter(Collision collision)
    {
        if (currentInkColor == StampInkColor.None || currentStampSO == null)
        {
            Debug.Log("Ignoring collision because ink color is None or no stamp is set");
            return;
        }

        var receiver = collision.gameObject.GetComponent<StampReceiver>();
        if (receiver != null && collision.relativeVelocity.magnitude > stampForceThreshold)
        { 
            currentReceiver = receiver;
            isInContact = true;
            
            // Store the contact point from the collision
            contactPoint = collision.contacts[0].point;
            
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
        }
    }

    private Color GetColorFromInk(StampInkColor ink)
    {
        return ink switch
        {
            StampInkColor.Red => Color.red,
            StampInkColor.Green => Color.green,
            StampInkColor.Black => Color.black,
            _ => Color.white
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
            isInContact ? contactPoint : stampHead.position, // Use contact point if in contact
            stampHead.rotation,
            currentStampSO,
            currentInkColor,
            stampHead.lossyScale
        );

        OnStamp?.Invoke(data);
        
        if (isInContact && currentReceiver != null)
        {
            currentReceiver.HandleStamp(data);
        }
    }
    
    public bool IsInContactWithReceiver()
    {
        return isInContact && currentReceiver != null;
    }
}