using System;
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
    private readonly Dictionary<string, List<GameObject>> _objectPoolByName = new Dictionary<string, List<GameObject>>();

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
                _objectPoolByName.Add(poolObj.name, new List<GameObject>());
                for (int i = 0; i < poolObj.poolSize; ++i)
                {
                    GameObject go = Instantiate(poolObj.prefab, poolGO.transform, true);
                    go.name = $"{poolObj.name}_{_objectPoolByName[poolObj.name].Count:000}";
                    go.SetActive(false);
                    _objectPoolByName[poolObj.name].Add(go);
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
        foreach (var pool in _objectPoolByName.Values)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].SetActive(false);
            }
        }
    }

    public void RecycleObject(GameObject go)
    {
        go.SetActive(false);
    }

    private GameObject GetNextObject(string poolName)
    {
        List<GameObject> pooledObjects = _objectPoolByName[poolName];
        foreach (GameObject go in pooledObjects)
        {
            if (go == null)
            {
                Debug.LogError("Pooled Object Is NULL");
                continue;
            }

            if (go.activeInHierarchy)
            {
                continue; // keep looking
            }

            return go;
        }

        Debug.LogError("Object Pool Depleted: No Unused Objects To Return");
        return null;
    }
}
