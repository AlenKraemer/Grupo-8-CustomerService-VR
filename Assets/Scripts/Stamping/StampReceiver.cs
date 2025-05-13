using System;
using UnityEngine;

public class StampReceiver : PaperworkBase
{
    [SerializeField] private GameObject _childs;
    [SerializeField] private GameObject stampTemplate; // Reference to a template stamp GameObject

    private void Awake()
    {
        paperworkType = PaperworkType.stamp;
        isDone = false;
    }

    private void Start()
    {
        GameManager.Instance.questManager.onButtonPressed += Retreat;
    }

    private void Retreat()
    {
        if (!isDone) return;
        GameManager.Instance.stampSpawn.Finished(this);
    }

    public void HandleStamp(StampData data)
    {
        if (_childs == null)
        {
            Debug.LogError("Childs GameObject not assigned in StampReceiver");
            return;
        }
        
        // Instantiate the template inside the childs GameObject
        GameObject stamp = Instantiate(stampTemplate, _childs.transform);
    
        // Position the stamp at the exact collision point
        var position = data.position;
        stamp.transform.position = new Vector3(position.x, position.y, position.z);

        // Ensure y position is exactly 0 (additional guarantee)
        stamp.transform.position = new Vector3(stamp.transform.position.x, stamp.transform.position.y, stamp.transform.position.z);


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
        Color inkColor = GetColorFromInk(data.inkColor);

        // Create a completely new material instance
        Material newMaterial = new Material(data.stampSO._stampMaterial);
    
        // Directly set the color property as requested
        newMaterial.color = inkColor;
    
        // Set material to the renderer
        stampRenderer.material = newMaterial;
    
        // Name the stamp with color information for easy identification
        stamp.name = $"Stamp_{data.inkColor}_{System.DateTime.Now.Ticks}";
        isDone = true;
    }

    private Color GetColorFromInk(StampInkColor ink)
    {
        return ink switch
        {
            StampInkColor.Red => Color.red,
            StampInkColor.Green => Color.green,
            StampInkColor.Black => Color.black,
            _ => Color.gray
        };
    }

    private void OnDisable()
    {
        paperworkType = PaperworkType.stamp;
        isDone = false;
    }
}