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
    
        // Log incoming data for debugging
        Debug.Log($"Stamp requested with ink color: {data.inkColor}, StampSO: {data.stampSO.name}");
    
        // Instantiate the template inside the childs GameObject
        GameObject stamp = Instantiate(stampTemplate, _childs.transform);
    
        // Position the stamp at the exact collision point
        var position = data.position;
        stamp.transform.localPosition = new Vector3(position.x, 0, position.z);
    
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
        Debug.Log($"Using color: {inkColor} for stamp");
    
        // Create a completely new material instance
        Material newMaterial = new Material(data.stampSO._stampMaterial);
    
        // Set the main color property of the material
        newMaterial.SetColor("_Color", inkColor);
    
        // If the shader uses "_BaseColor" property (Standard shader in URP/HDRP)
        if (newMaterial.HasProperty("_BaseColor"))
        {
            newMaterial.SetColor("_BaseColor", inkColor);
        }
    
        // Set material to the renderer
        stampRenderer.material = newMaterial;
    
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