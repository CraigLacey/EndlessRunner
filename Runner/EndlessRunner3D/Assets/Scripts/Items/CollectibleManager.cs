using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private GameObject _collectiblePrefab;

    public void Initialize()
    {
        Debug.Log($"{nameof(CollectibleManager)} -> Initializing");
    }

    // TODO: Async
    public GameObject SpawnCollectible(Vector3 spawnPos)
    {
        return Instantiate(_collectiblePrefab, spawnPos, Quaternion.identity);
    }
}
