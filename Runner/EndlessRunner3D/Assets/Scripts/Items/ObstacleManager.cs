using System.Threading.Tasks;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private const string ObstaclePoolName = "Obstacle";

    public Task InitializeAsync()
    {
        Debug.Log($"{nameof(ObstacleManager)} -> Initializing");
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();

        if(_objectPoolManager == null)
        {
            Debug.LogError($"{nameof(ObstacleManager)} -> ObjectPoolManager is not ready");
            return Task.FromException(new System.Exception("ObjectPoolManager is not ready"));
        }

        return Task.CompletedTask;
    }

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

    public void RecycleObstacle(GameObject itemGO)
    {
        _objectPoolManager.RecycleObject(itemGO, ObstaclePoolName);
    }
}
