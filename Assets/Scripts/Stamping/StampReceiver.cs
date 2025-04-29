using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampReceiver : MonoBehaviour
{
    [SerializeField] private GameObject _childs;
    [SerializeField] private GameObject stampTemplate; // Reference to a template stamp GameObject

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
        Vector3 position = data.position;
        stamp.transform.position = position;

        // Get the renderer component
        Renderer stampRenderer = stamp.GetComponent<Renderer>();
        if (stampRenderer == null)
        {
            Debug.LogError("No Renderer component found on stampTemplate");
            return;
        }

        // Create a completely new material instance
        Material stampMaterial = new Material(data.stampSO._stampMaterial);
    
        // Apply the ink color to the new material
        Color inkColor = GetColorFromInk(data.inkColor);
        stampMaterial.color = inkColor;
    
        // Assign the new material to the renderer
        stampRenderer.sharedMaterial = null; // Break any shared references
        stampRenderer.material = stampMaterial;

        Debug.Log($"Applying stamp with color: {inkColor}, ink type: {data.inkColor}");
    
        // Name the stamp with color information for easy identification
        stamp.name = $"Stamp_{data.inkColor}_{System.DateTime.Now.Ticks}";
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
}