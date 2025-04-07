using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Object Pool Manager is responsible for managing object pools.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    /// <summary>
    /// Data structure to hold information about each pool for use in Inspector
    /// </summary>
    [Serializable]
    public class PoolData
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }
    [SerializeField] private List<PoolData> _pools = new List<PoolData>();

    public bool IsInitialized => _isInitialized;
    private bool _isInitialized = false;

    private readonly Dictionary<string, ConcurrentQueue<GameObject>> _objectPoolByName = new();
    private static readonly object s_queueLock = new();
    private GameObject _poolRootObj = null;

    /// <summary>
    /// Initialize the Object Pool Manager asynchronously.
    /// </summary>
    /// <returns></returns>
    public Task InitializePoolAsync()
    {
        _poolRootObj = new GameObject("Object Pool");
        _poolRootObj.transform.SetParent(AppLoader.SystemRoot, true);

        // Load pools from data set in the inspector
        CreatePoolsFromInspectorData();

        _isInitialized = true;
        ServiceLocator.Register<ObjectPoolManager>(this);

        Debug.Log("Object Pool Manager Initialized");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a pool to the object pool manager.
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="objectsToPool"></param>
    public void AddPool(string poolName, List<GameObject> objectsToPool)
    {
        // Validate the data before adding the pool
        if (string.IsNullOrEmpty(poolName))
        {
            Debug.LogError("Pool name cannot be null or empty");
            return;
        }

        if(objectsToPool == null || objectsToPool.Count == 0)
        {
            Debug.LogError("Objects to pool cannot be null or empty");
            return;
        }

        if(_objectPoolByName.ContainsKey(poolName))
        {
            Debug.LogError($"Pool with name {poolName} already exists");
            return;
        }

        lock (s_queueLock)
        {
            GameObject poolGO = new GameObject(poolName);
            poolGO.transform.SetParent(_poolRootObj.transform);
            _objectPoolByName.Add(poolName, new ConcurrentQueue<GameObject>());
            foreach (GameObject obstacle in objectsToPool)
            {
                obstacle.transform.SetParent(poolGO.transform);
                obstacle.SetActive(false);
                _objectPoolByName[poolName].Enqueue(obstacle);
            }
            Debug.Log($"Loaded {objectsToPool.Count} obstacles into pool: {poolName}");
        }
    }

    /// <summary>
    /// Get an object from the pool by name.
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Deactivate all objects that are pooled
    /// </summary>
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

    /// <summary>
    /// Recycle an object back into it's pool.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="poolName"></param>
    public void RecycleObject(GameObject go, string poolName)
    {
        lock (s_queueLock)
        {
            go.SetActive(false);
            _objectPoolByName[poolName].Enqueue(go);
        }
    }

    /// <summary>
    /// Create pools from the data set in the inspector.
    /// </summary>
    private void CreatePoolsFromInspectorData()
    {
        foreach (PoolData poolData in _pools)
        {
            if (!_objectPoolByName.ContainsKey(poolData.name))
            {
                Debug.Log($"Creating Pool: {poolData.name} Size: {poolData.poolSize}");
                GameObject poolGO = new GameObject(poolData.name);
                poolGO.transform.SetParent(_poolRootObj.transform);
                _objectPoolByName.Add(poolData.name, new ConcurrentQueue<GameObject>());
                for (int i = 0; i < poolData.poolSize; ++i)
                {
                    GameObject go = Instantiate(poolData.prefab, poolGO.transform, true);
                    go.name = $"{poolData.name}_{_objectPoolByName[poolData.name].Count:000}";
                    go.SetActive(false);
                    lock (s_queueLock)
                    {
                        _objectPoolByName[poolData.name].Enqueue(go);
                    }
                }
            }
            else
            {
                Debug.Log("WARNING: Attempting to create multiple pools with the same name: " + poolData.name);
                continue;
            }
        }
    }

    /// <summary>
    /// Get the next available object from the pool.
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
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
