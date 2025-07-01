using UnityEngine;
using System.Collections.Generic;

namespace Stamping
{
    /// <summary>
    /// Manages ink zones to change the ink color of stamping tools when they are in proximity.
    /// </summary>
    public class InkChangerPad : MonoBehaviour
    {
        [Header("Color Zone Colliders")]
        [SerializeField] private Collider _redZoneCollider;
        [SerializeField] private Collider _greenZoneCollider;
        [SerializeField] private Collider _blackZoneCollider;
        
        [Header("Detection Settings")]
        [SerializeField] private bool _debugMode = true;
        
        // Dictionary to map colliders to their respective ink colors
        private readonly Dictionary<Collider, StampInkColor> _zoneMap = new();
        
        private void Awake()
        {
            // Setup each zone with the appropriate ink color
            SetupZone(_redZoneCollider, StampInkColor.Red);
            SetupZone(_greenZoneCollider, StampInkColor.Green);
            SetupZone(_blackZoneCollider, StampInkColor.Black);
        }
        
        private void SetupZone(Collider zoneCollider, StampInkColor color)
        {
            if (zoneCollider == null) return;
            
            // Store the mapping
            _zoneMap[zoneCollider] = color;
            
            if (_debugMode) Debug.Log($"Setup zone {zoneCollider.name} for color {color}", zoneCollider);
        }
        
        /// <summary>
        /// Gets the ink color associated with the given otherCollider if it's one of our zones
        /// </summary>
        /// <param name="otherCollider">The otherCollider to check</param>
        /// <returns>The ink color for the zone, or None if not a valid zone</returns>
        public StampInkColor GetInkColorForCollider(Collider otherCollider)
        {
            return _zoneMap.GetValueOrDefault(otherCollider, StampInkColor.None);
        }
        
        private void OnDrawGizmos()
        {
            if (!_debugMode) return;
            
            // Draw zone bounds for debugging
            DrawZoneBounds(_redZoneCollider, Color.red);
            DrawZoneBounds(_greenZoneCollider, Color.green);
            DrawZoneBounds(_blackZoneCollider, Color.black);
        }
        
        private static void DrawZoneBounds(Collider otherCollider, Color color)
        {
            if (otherCollider == null) return;
            
            var gizmoColor = color;
            gizmoColor.a = 0.3f; // Set transparency
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(otherCollider.bounds.center, otherCollider.bounds.size);
            
            // Draw outline
            Gizmos.color = color;
            Gizmos.DrawWireCube(otherCollider.bounds.center, otherCollider.bounds.size);
        }
    }
}