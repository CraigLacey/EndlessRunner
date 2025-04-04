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

    private GameObject _prop;
    private Action TileExit;
    private CollectibleManager _collectibleManager;

    public void Initialize(Action onTileExit)
    {
        _collectibleManager = ServiceLocator.Get<CollectibleManager>();

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
                SpawnCollectible();
                break;
            default:
                Debug.Log($"SpawnItem for unhandled TileType {_tileType}");
                break;
        }
    }

    private void HandleTileExit()
    {
        Destroy(_prop);
        TileExit?.Invoke();
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

        _prop = _collectibleManager.SpawnCollectible(itemPos);
    }
}
