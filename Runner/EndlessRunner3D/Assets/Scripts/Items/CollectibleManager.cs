using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private const string CollectiblePoolName = "Collectible";

    public void Initialize()
    {
        Debug.Log($"{nameof(CollectibleManager)} -> Initializing");
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

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

    public void RecycleCollectible(GameObject collectible)
    {
        _objectPoolManager.RecycleObject(collectible, CollectiblePoolName);
    }
}
