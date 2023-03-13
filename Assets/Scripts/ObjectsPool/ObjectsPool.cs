using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ObjectsPool
{
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        public event Action<T> OnCreate;
        public T prefab;
        public Transform container;

        private List<T> _pool;

        public ObjectsPool(T prefab, int count, Transform container)
        {
            this.prefab = prefab;
            this.container = container;

            CreatePool(count);
        }

        public T Get()
        {
            if (HasElement(out var element))
            {
                return element;
            }
            else
            {
                var newObj = CreateObject(true);
                OnCreate?.Invoke(newObj);
                return newObj;
            }
        }

        public List<T> GetAll()
        {
            return _pool;
        } 

        public void Return(T element)
        {
            element.gameObject.transform.SetParent(container);
            element.gameObject.SetActive(false);
            element.gameObject.transform.localScale = Vector3.one;
        }

        private bool HasElement(out T element)
        {
            foreach (var obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    element = obj;
                    obj.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = UnityEngine.Object.Instantiate(prefab, container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }
    }
}