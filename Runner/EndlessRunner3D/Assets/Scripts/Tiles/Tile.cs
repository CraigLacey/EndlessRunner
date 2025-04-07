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

    public enum EItemType
    {
        None = 0,
        Collectible = 1,
        Obstacle = 2,
        Prop = 3,
    }
    private struct TileItem
    {
        public GameObject ItemGO;
        public EItemType ItemType;
    }

    private TileItem _tileItem;
    private Action TileExit;
    private CollectibleManager _collectibleManager;
    private ObstacleManager _obstacleManager;

    private int _obstacleSpawnChance = 25; // 25% chance to spawn an obstacle
    private int _obstacleChanceIncrement = 5; // Increase the chance by 5% each time
    private int _maxObstacleChance = 50; // Maximum chance to spawn an obstacle

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

    public void IncreaseObstacleSpawnChance()
    {
        _obstacleSpawnChance += _obstacleChanceIncrement;
        if (_obstacleSpawnChance > _maxObstacleChance)
        {
            _obstacleSpawnChance = _maxObstacleChance;
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

    internal void ClearItems()
    {
        switch (_tileItem.ItemType)
        {
            case EItemType.Collectible:
                // If the collectible is active, recycle it
                if (_tileItem.ItemGO.activeInHierarchy)
                {
                    _collectibleManager.RecycleCollectible(_tileItem.ItemGO);
                }
                break;
            case EItemType.Obstacle:
                _obstacleManager.RecycleObstacle(_tileItem.ItemGO);
                break;
            default:
                Debug.Log($"HandleTileExit for unhandled ItemType {_tileItem.ItemType}");
                break;
        }
    }

    private void HandleTileExit()
    {
        ClearItems();
        TileExit?.Invoke();
    }

    private void SpawnPathItem()
    {
        int randNum = UnityEngine.Random.Range(0, 100);
        if(randNum < _obstacleSpawnChance)
        {
            // Spawn an obstacle
            SpawnObstacle();
        }
        else
        {
            // Spawn a collectible item
            SpawnCollectible();
        }

        if(_tileItem.ItemGO == null)
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
        _tileItem.ItemGO = _obstacleManager.SpawnObstacle(itemPos);
        _tileItem.ItemType = EItemType.Obstacle;
        Debug.Log($"Spawned Obstacle {_tileItem.ItemGO.name} on {gameObject.name}");
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

        _tileItem.ItemGO = _collectibleManager.SpawnCollectible(itemPos);
        _tileItem.ItemType = EItemType.Collectible;
        Debug.Log($"Spawned Collectible {_tileItem.ItemGO.name} on {gameObject.name}");
    }

    private void SpawnProp()
    {
        
    }
}
