using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public class PoolData
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }
    public List<PoolData> pools = new List<PoolData>();

    public bool IsInitialized => _isInitialized;
    private bool _isInitialized = false;
    private readonly Dictionary<string, ConcurrentQueue<GameObject>> _objectPoolByName = new();
    private static readonly object s_queueLock = new();
    private GameObject _poolRootObj = null;

    public async Task InitializePoolAsync()
    {
        _poolRootObj = new GameObject("Object Pool");
        _poolRootObj.transform.SetParent(AppLoader.SystemRoot, true);

        // Load pools from data set in the inspector
        CreatePoolsFromInspectorData();

        // Load obstacle data from disk
        await LoadObstaclesFromDiskAsync();

        _isInitialized = true;
        ServiceLocator.Register<ObjectPoolManager>(this);

        Debug.Log("Object Pool Manager Initialized");
        return;
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

    private void CreatePoolsFromInspectorData()
    {
        foreach (PoolData poolData in pools)
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

    private async Task LoadObstaclesFromDiskAsync()
    {
        Debug.Log("Loading obstacles from disk...");
        // Loading obstacles from disk
        string obstacleDataDirectory = "ObstacleData";
        string obstaclePoolName = "Obstacle";
        List<Task> loadTasks = new();
        List<GameObject> spawnedObstacles = new();

#if UNITY_ANDROID && !UNITY_EDITOR
        string dataPath = Path.Combine(Application.streamingAssetsPath, obstacleDataDirectory);
        using (UnityWebRequest request = UnityWebRequest.Get(dataPath))
        {
            await WebRequestUtils.SendWebRequestAsync(request);
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to list obstacle data directory: {request.error}");
                return;
            }

            string fileListString = request.downloadHandler.text;
            string[] files = fileListString.Split('\n');
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                {
                    loadTasks.Add(LoadObstacleDataAndInstantiateAsync(Path.Combine(Application.streamingAssetsPath, obstacleDataDirectory, file.Trim()), spawnedObstacles));
                }
            }
        }
#else
        string dataPath = Path.Combine(Application.streamingAssetsPath, obstacleDataDirectory);
        if (Directory.Exists(dataPath))
        {
            string[] files = Directory.GetFiles(dataPath, "*.json");
            foreach (string file in files)
            {
                loadTasks.Add(LoadObstacleDataAndInstantiateAsync(file, spawnedObstacles));
            }
        }
        else
        {
            Debug.LogError($"Obstacle data directory not found: {dataPath}");
            return;
        }
#endif

        await Task.WhenAll(loadTasks);

        // Queue the spawned obstacles into the object pool
        lock (s_queueLock)
        {
            GameObject poolGO = new GameObject(obstaclePoolName);
            poolGO.transform.SetParent(_poolRootObj.transform);
            _objectPoolByName.Add(obstaclePoolName, new ConcurrentQueue<GameObject>());
            foreach (GameObject obstacle in spawnedObstacles)
            {
                obstacle.transform.SetParent(poolGO.transform);
                obstacle.SetActive(false);
                _objectPoolByName[obstaclePoolName].Enqueue(obstacle);
            }
            Debug.Log($"Loaded {spawnedObstacles.Count} obstacles into pool: {obstaclePoolName}");
        }
    }

    private async Task LoadObstacleDataAndInstantiateAsync(string filePath, List<GameObject> spawnedObstacles)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            await WebRequestUtils.SendWebRequestAsync(request);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                ObstacleData data = JsonUtility.FromJson<ObstacleData>(json);

                if (!string.IsNullOrEmpty(data.prefabName))
                {
                    Debug.Log($"Loading obstacle prefab: {data.prefabName} from {filePath}");
                    GameObject prefab = Resources.Load<GameObject>(data.prefabName);
                    if (prefab != null)
                    {
                        for (int i = 0; i < data.poolSize; ++i)
                        {
                            GameObject obstacleInstance = Instantiate(prefab, data.position, data.rotation);
                            obstacleInstance.transform.localScale = data.scale;
                            spawnedObstacles.Add(obstacleInstance);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Prefab not found in Resources: {data.prefabName}");
                    }
                }
                else
                {
                    Debug.LogError($"Prefab name not specified in data: {filePath}");
                }
            }
            else
            {
                Debug.LogError($"Failed to load obstacle data from {filePath}: {request.error}");
            }
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
