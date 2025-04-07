using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// CollectibleManager is responsible for managing collectibles in the game.
/// </summary>
public class CollectibleManager : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private const string CollectiblePoolName = "Collectible";

    /// <summary>
    /// Initialize the CollectibleManager.
    /// </summary>
    /// <returns></returns>
    public Task InitializeAsync()
    {
        Debug.Log($"{nameof(CollectibleManager)} -> Initializing");
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();

        if(_objectPoolManager == null)
        {
            Debug.LogError($"{nameof(CollectibleManager)} -> ObjectPoolManager is not ready");
            return Task.FromException(new System.Exception("ObjectPoolManager is not ready"));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Spawn a collectible at the specified position.
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <returns></returns>
    public GameObject SpawnCollectible(Vector3 spawnPos)
    {
        if (_objectPoolManager == null || !_objectPoolManager.IsInitialized)
        {
            Debug.LogError($"{nameof(CollectibleManager)} -> ObjectPoolManager is not ready");
            return null;
        }
        
        // Get the object from the pool
        GameObject collectible = _objectPoolManager.GetObjectFromPool(CollectiblePoolName);
        collectible.transform.position = spawnPos;
        collectible.SetActive(true);
        Collectible c = collectible.GetComponent<Collectible>();
        if (!c.Initialized)
        {
            c.Initialize();
        }
        return collectible;
    }

    /// <summary>
    /// Recycle a collectible back to it's pool.
    /// </summary>
    /// <param name="collectible"></param>
    public void RecycleCollectible(GameObject collectible)
    {
        _objectPoolManager.RecycleObject(collectible, CollectiblePoolName);
    }
}
