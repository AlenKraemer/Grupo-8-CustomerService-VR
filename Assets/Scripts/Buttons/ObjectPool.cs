using System.Collections;
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
            pooledObject.SetActive(true);
            _availablePool.RemoveAt(0);
        }
        else if (_usedPool.Count + _availablePool.Count < _maxObjects)
        {
            // Instantiate new if under limit
            pooledObject = Instantiate(_originalPrefab);
        }
        else
        {
            // Limit reached
            return null;
        }

        _usedPool.Add(pooledObject);
        return pooledObject;
    }

    public void ReturnToPool(GameObject recycle)
    {
        _usedPool.Remove(recycle);
        recycle.SetActive(false);
        _availablePool.Add(recycle);
    }

    public void Clear()
    {
        //_usedPool.Clear();
        _availablePool.Clear();
    }
}