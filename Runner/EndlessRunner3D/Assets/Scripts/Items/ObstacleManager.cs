using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ObstacleManager is responsible for managing obstacles in the game.
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private const string ObstaclePoolName = "Obstacle";

    /// <summary>
    /// Initialize the ObstacleManager and load obstacles from disk asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        Debug.Log($"{nameof(ObstacleManager)} -> Initializing");
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();

        if (_objectPoolManager == null)
        {
            Debug.LogError($"{nameof(ObstacleManager)} -> ObjectPoolManager is not ready");
            return;
        }

        await LoadObstaclesFromDiskAsync();

        return;
    }

    /// <summary>
    /// Spawn an obstacle at the specified position.
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <returns></returns>
    public GameObject SpawnObstacle(Vector3 spawnPos)
    {
        if (_objectPoolManager == null || !_objectPoolManager.IsInitialized)
        {
            Debug.LogError($"{nameof(ObstacleManager)} -> ObjectPoolManager is not ready");
            return null;
        }

        GameObject obstacleGO = _objectPoolManager.GetObjectFromPool(ObstaclePoolName);
        if (obstacleGO != null)
        {
            obstacleGO.transform.position = spawnPos;
            obstacleGO.SetActive(true);
            Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.Initialize();
            }
            return obstacleGO;
        }
        else
        {
            Debug.LogError($"Failed to spawn obstacle: {nameof(obstacleGO)} is null");
            return null;
        }
    }

    /// <summary>
    /// Recycle an obstacle back to the Obstacle pool.
    /// </summary>
    /// <param name="itemGO"></param>
    public void RecycleObstacle(GameObject itemGO)
    {
        _objectPoolManager.RecycleObject(itemGO, ObstaclePoolName);
    }

    /// <summary>
    /// Load obstacles from disk asynchronously.
    /// </summary>
    /// <returns></returns>
    private async Task LoadObstaclesFromDiskAsync()
    {
        Debug.Log("Loading obstacles from disk...");

        string obstacleDataDirectory = "ObstacleData";
        string obstaclePoolName = "Obstacle";
        List<Task> loadTasks = new();
        List<GameObject> spawnedObstacles = new();

        // Load from StreamingAssets. On device use UnityWebRequest to get the file list.
#if UNITY_ANDROID && !UNITY_EDITOR
        string indexPath = Path.Combine(Application.streamingAssetsPath, obstacleDataDirectory, "obstacle_manifest.txt");
        using (UnityWebRequest indexRequest = UnityWebRequest.Get(indexPath))
        {
            await WebRequestUtils.SendWebRequestAsync(indexRequest);
            if (indexRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to list obstacle data directory: {indexRequest.error}");
                return;
            }

            string[] fileNames = indexRequest.downloadHandler.text.Split('\n');
            foreach (string fileName in fileNames)
            {
                string trimmedFileName = fileName.Trim();
                if (!string.IsNullOrEmpty(trimmedFileName) && trimmedFileName.EndsWith(".json"))
                {
                    loadTasks.Add(LoadObstacleDataAndInstantiateAsync(Path.Combine(Application.streamingAssetsPath, obstacleDataDirectory, trimmedFileName), spawnedObstacles));
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

        _objectPoolManager.AddPool(obstaclePoolName, spawnedObstacles);
    }

    /// <summary>
    /// Load obstacle data from a JSON file and instantiate the obstacles that will then be added to the pool.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="spawnedObstacles"></param>
    /// <returns></returns>
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
}

