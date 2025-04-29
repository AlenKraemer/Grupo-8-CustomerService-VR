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
        stamp.transform.position = new Vector3(position.x, 0, position.z);

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

        // Force destroy any existing material to prevent shared references
        if (stampRenderer.material != null)
        {
            Destroy(stampRenderer.material);
        }

        // Create a completely new material instance
        Material stampMaterial = new Material(data.stampSO._stampMaterial);

        // Get the color based on ink type
        Color inkColor = GetColorFromInk(data.inkColor);
        Debug.Log($"Using color: {inkColor} for stamp");

        // Apply the ink color to the new material
        stampMaterial.color = inkColor;

        // Assign the new material to the renderer
        stampRenderer.material = stampMaterial;

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