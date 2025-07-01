using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Custom
{
    public class CustomUpdateManager : MonoBehaviour
    {
        private static CustomUpdateManager _instance;
        public static CustomUpdateManager Instance => _instance;

        private readonly List<IUpdatable> _updatables = new();

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            foreach (var item in _updatables)
            {
                item.OnUpdate();
            }
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        public void Subscribe(IUpdatable item)
        {
            _updatables.Add(item);
        }

        public void UnSuscribe(IUpdatable item)
        {
            _updatables.Remove(item);
        }
    }
}
