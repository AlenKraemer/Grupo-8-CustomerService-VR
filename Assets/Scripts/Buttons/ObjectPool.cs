using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] public GameObject _originalPrefab;
    [SerializeField] private List<GameObject> _usedPool;
    [SerializeField] private List<GameObject> _availablePool;
    [SerializeField] private int _maxObjects;

    public GameObject GetObject()
    {
        GameObject pooledObject;

        if (_availablePool.Count > 0)
        {
            // Reuse from available pool
            pooledObject = _availablePool[0];
            Debug.Log($"[ObjectPool] Getting from pool: {pooledObject.name} at position {pooledObject.transform.position}");
        
            pooledObject.SetActive(true);
            Debug.Log($"[ObjectPool] After activation: {pooledObject.name} at position {pooledObject.transform.position}");
        
            _availablePool.RemoveAt(0);
        }
        else if (_usedPool.Count + _availablePool.Count < _maxObjects)
        {
            // Instantiate new if under limit
            Debug.Log($"[ObjectPool] Creating new instance from prefab at position {_originalPrefab.transform.position}");
            pooledObject = Instantiate(_originalPrefab);
            Debug.Log($"[ObjectPool] After instantiation: {pooledObject.name} at position {pooledObject.transform.position}");
            Debug.Log($"[ObjectPool] Parent: {pooledObject.transform.parent?.name ?? "null"}, localPosition: {pooledObject.transform.localPosition}");
        
            // Check if original prefab has any parent setting scripts
            var components = _originalPrefab.GetComponents<MonoBehaviour>();
            Debug.Log($"[ObjectPool] Original prefab has {components.Length} MonoBehaviour components");
        }
        else
        {
            // Limit reached
            return null;
        }
        
        // After initializing pooledObject, add this:
        var allComponents = pooledObject.GetComponentsInChildren<MonoBehaviour>(true);
        Debug.Log($"[ObjectPool] Object has {allComponents.Length} MonoBehaviours in hierarchy:");
        foreach (var component in allComponents)
        {
            Debug.Log($"[ObjectPool] Component: {component.GetType().Name} on GameObject: {component.gameObject.name}");
        }
    
        // Check for XR-related components specifically
        var xrComponents = pooledObject.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable>(true);
        if (xrComponents.Length > 0)
        {
            Debug.Log($"[ObjectPool] Found {xrComponents.Length} XR Interactable components:");
            foreach (var component in xrComponents)
            {
                Debug.Log($"[ObjectPool] XR Component: {component.GetType().Name} on {component.gameObject.name}");
            }
        }

        _usedPool.Add(pooledObject);
        return pooledObject;
    }

    public void ReturnToPool(GameObject recycle)
    {
        // Add position logging before removing from used pool
        Debug.Log($"[ObjectPool] Before returning to pool: {recycle.name} at position {recycle.transform.position}");
        Debug.Log($"[ObjectPool] Parent before: {recycle.transform.parent?.name ?? "null"}, localPosition: {recycle.transform.localPosition}");
    
        _usedPool.Remove(recycle);
    
        // Log position right before deactivation
        Debug.Log($"[ObjectPool] Right before deactivation: {recycle.name} at position {recycle.transform.position}");
    
        recycle.SetActive(false);
    
        // Log position after deactivation
        Debug.Log($"[ObjectPool] After deactivation: {recycle.name} at position {recycle.transform.position}");
    
        _availablePool.Add(recycle);
    
        // Log final position in pool
        Debug.Log($"[ObjectPool] Final position in pool: {recycle.name} at position {recycle.transform.position}");
    }

    public void Clear()
    {
        //_usedPool.Clear();
        _availablePool.Clear();
    }
}