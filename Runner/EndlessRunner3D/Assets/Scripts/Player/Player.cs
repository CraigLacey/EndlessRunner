using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    private Action ObstacleCollision;

    public void Initialize(Action onObstacleCollision)
    {
        Debug.Log($"{nameof(Player)} Initializing ...");

        ObstacleCollision += onObstacleCollision;
        _playerController.Initialize();

        Debug.Log($"{nameof(Player)} Initialized");
    }

    public void StartRunning()
    {
        _playerController.StartRunning();
    }

    public void StopRunning()
    {
        _playerController.StopRunning();
    }

    public void IncreaseSpeed()
    {
        _playerController.IncreaseSpeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log($"{nameof(Player)} -> Collected an item!");
            other.GetComponent<Collectible>().Collect();
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log($"{nameof(Player)} -> Hit an obstacle!");
            ObstacleCollision?.Invoke();
        }
    }
}
