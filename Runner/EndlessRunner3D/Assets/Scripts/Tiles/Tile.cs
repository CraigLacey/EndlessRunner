using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum ETileType
    {
        None = 0,
        Path = 1,
        Sideline = 2,
    }
    [SerializeField] private ETileType _tileType = ETileType.None;
    [SerializeField] private TriggerBox _triggerBox;

    private GameObject _tileItem;
    private Action TileExit;
    private CollectibleManager _collectibleManager;
    private ObstacleManager _obstacleManager;

    public void Initialize(Action onTileExit)
    {
        _collectibleManager = ServiceLocator.Get<CollectibleManager>();
        _obstacleManager = ServiceLocator.Get<ObstacleManager>();

        TileExit += onTileExit;
        if (_triggerBox != null)
        {
            _triggerBox.Initialize(HandleTileExit);
        }
    }

    public void SpawnItem()
    {
        switch (_tileType)
        {
            case ETileType.Path:
                SpawnPathItem();
                break;
            case ETileType.Sideline:
                SpawnProp();
                break;
            default:
                Debug.Log($"SpawnItem for unhandled TileType {_tileType}");
                break;
        }
    }

    private void HandleTileExit()
    {
        _collectibleManager.RecycleCollectible(_tileItem);
        TileExit?.Invoke();
    }

    private void SpawnPathItem()
    {
        int randNum = UnityEngine.Random.Range(0, 100);
        if(randNum < 25)
        {
            // Spawn an obstacle
            SpawnObstacle();
        }
        else
        {
            // Spawn a collectible item
            SpawnCollectible();
        }

        if(_tileItem == null)
        {
            Debug.Log("SpawnItem failed: _tileItem is null");
        }
    }

    private void SpawnObstacle()
    {
        Vector3 tilePos = transform.position;
        Vector3 itemPos = Vector3.zero;
        // Adjust the X position to be left/middle/right randomly
        int randPos = UnityEngine.Random.Range(0, 100);
        if (randPos < 33)
        {
            itemPos = new Vector3(tilePos.x - 2.5f, tilePos.y + 1, tilePos.z);
        }
        else if (randPos < 66)
        {
            itemPos = new Vector3(tilePos.x, tilePos.y + 1, tilePos.z);
        }
        else
        {
            itemPos = new Vector3(tilePos.x + 2.5f, tilePos.y + 1, tilePos.z);
        }
        _tileItem = _obstacleManager.SpawnObstacle(itemPos);
        Debug.Log($"Spawned Obstacle {_tileItem.name} on {gameObject.name}");
    }

    private void SpawnCollectible()
    {
        Vector3 tilePos = transform.position;
        Vector3 itemPos = Vector3.zero;

        // Adjust the X position to be left/middle/right randomly
        int randPos = UnityEngine.Random.Range(0, 100);
        if (randPos < 33)
        {
            itemPos = new Vector3(tilePos.x - 2.5f, tilePos.y + 1, tilePos.z);
        }
        else if (randPos < 66)
        {
            itemPos = new Vector3(tilePos.x, tilePos.y + 1, tilePos.z);
        }
        else
        {
            itemPos = new Vector3(tilePos.x + 2.5f, tilePos.y + 1, tilePos.z);
        }

        _tileItem = _collectibleManager.SpawnCollectible(itemPos);
        Debug.Log($"Spawned Collectible {_tileItem.name} on {gameObject.name}");
    }

    private void SpawnProp()
    {
        
    }
}
