using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private GameObject _collectiblePrefab;

    public void Initialize()
    {
        Debug.Log($"{nameof(CollectibleManager)} -> Initializing");
    }

    public void SpawnCollectible(Vector3 spawnPos)
    {
        Instantiate(_collectiblePrefab, spawnPos, Quaternion.identity);
    }
}
