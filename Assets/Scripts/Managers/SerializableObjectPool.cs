using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [System.Serializable]
    public class SerializableObjectPool
    {
        [SerializeField] private GameObject _originalPrefab;
        [SerializeField] private int _maxObjects = 10;
        [SerializeField] private Transform _parentTransform;
        
        private List<GameObject> _usedPool = new List<GameObject>();
        private List<GameObject> _availablePool = new List<GameObject>();

        public void Initialize(GameObject prefab, int maxObjects, Transform parent = null)
        {
            _originalPrefab = prefab;
            _maxObjects = maxObjects;
            _parentTransform = parent;
        }

        public GameObject GetObject()
        {
            GameObject pooledObject;

            if (_availablePool.Count > 0)
            {
                pooledObject = _availablePool[0];
                pooledObject.SetActive(true);
                _availablePool.RemoveAt(0);
            }
            else if (_usedPool.Count + _availablePool.Count < _maxObjects)
            {
                pooledObject = Object.Instantiate(_originalPrefab, _parentTransform);
            }
            else
            {
                return null;
            }

            _usedPool.Add(pooledObject);
            return pooledObject;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (_usedPool.Contains(obj))
            {
                obj.SetActive(false);
                _usedPool.Remove(obj);
                _availablePool.Add(obj);
            }
        }

        public int AvailableCount => _availablePool.Count;
        public int UsedCount => _usedPool.Count;
        public int TotalCount => _availablePool.Count + _usedPool.Count;
    }
}
