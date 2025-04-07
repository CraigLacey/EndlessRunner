using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }
    public List<PooledObject> objectsToPool = new List<PooledObject>();

    public bool IsInitialized => _isInitialized;
    private bool _isInitialized = false;
    private readonly Dictionary<string, ConcurrentQueue<GameObject>> _objectPoolByName = new();
    private static readonly object s_queueLock = new();

    public Task InitializePoolAsync()
    {
        GameObject PoolManagerGO = new GameObject("Object Pool");
        PoolManagerGO.transform.SetParent(AppLoader.SystemRoot, true);
        foreach (PooledObject poolObj in objectsToPool)
        {
            if (!_objectPoolByName.ContainsKey(poolObj.name))
            {
                Debug.Log($"Creating Pool: {poolObj.name} Size: {poolObj.poolSize}");
                GameObject poolGO = new GameObject(poolObj.name);
                poolGO.transform.SetParent(PoolManagerGO.transform);
                _objectPoolByName.Add(poolObj.name, new ConcurrentQueue<GameObject>());
                for (int i = 0; i < poolObj.poolSize; ++i)
                {
                    GameObject go = Instantiate(poolObj.prefab, poolGO.transform, true);
                    go.name = $"{poolObj.name}_{_objectPoolByName[poolObj.name].Count:000}";
                    go.SetActive(false);
                    lock (s_queueLock)
                    {
                        _objectPoolByName[poolObj.name].Enqueue(go);
                    }
                }
            }
            else
            {
                Debug.Log("WARNING: Attempting to create multiple pools with the same name: " + poolObj.name);
                continue;
            }
        }

        _isInitialized = true;
        ServiceLocator.Register<ObjectPoolManager>(this);
        return Task.CompletedTask;
    }

    public GameObject GetObjectFromPool(string poolName)
    {
        GameObject ret = null;
        if (_objectPoolByName.ContainsKey(poolName))
        {
            ret = GetNextObject(poolName);
        }
        else
        {
            // No pool found by that name
            Debug.LogError("No Pool Exists With Name: " + poolName);
        }
        return ret;
    }

    public void DeactivateObjects()
    {
        lock (s_queueLock)
        {
            foreach (var pool in _objectPoolByName.Values)
            {
                int poolSize = pool.Count;
                for (int i = 0; i < poolSize; ++i)
                {
                    if (pool.TryDequeue(out GameObject go))
                    {
                        go.SetActive(false);
                        pool.Enqueue(go);
                    }
                }
            }
        }
    }

    public void RecycleObject(GameObject go, string poolName)
    {
        lock (s_queueLock)
        {
            go.SetActive(false);
            _objectPoolByName[poolName].Enqueue(go);
        }
    }

    private GameObject GetNextObject(string poolName)
    {
        lock (s_queueLock)
        {
            ConcurrentQueue<GameObject> pooledObjects = _objectPoolByName[poolName];
            if (pooledObjects.Count > 0)
            {
                pooledObjects.TryDequeue(out GameObject go);
                return go;
            }

            Debug.LogError($"{poolName} Object Pool Depleted: No Unused Objects To Return");
            return null;
        }
    }
}
