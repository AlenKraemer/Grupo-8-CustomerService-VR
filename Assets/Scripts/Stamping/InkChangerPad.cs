using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages ink zones to change the ink color of stamping tools upon trigger entry.
/// </summary>
public class InkChangerPad : MonoBehaviour
{
    [SerializeField]
    private StampInkColor inkColor; // The ink color associated with this zone

    /// <summary>
    /// Detects when a stamping tool enters a trigger and updates its ink color.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object entered zone trigger: {gameObject.name}");

        // Check if the entering object is or has a stamping tool
        var stampingTool = other.GetComponentInParent<StampingTool>();
        if (stampingTool == null)
        {
            Debug.Log($"Non-stamping tool entered: {other.name}");
            return;
        }

        // Set the stamping tool's ink color to this zone's color
        stampingTool.SetInkColor(inkColor);
        Debug.Log($"StampingTool '{stampingTool.name}' ink color set to {inkColor} in zone: {gameObject.name}");
    }
}