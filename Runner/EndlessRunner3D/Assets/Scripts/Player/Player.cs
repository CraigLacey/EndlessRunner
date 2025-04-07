using System;
using UnityEngine;

/// <summary>
/// Player class manages the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    private Action ObstacleCollision;

    /// <summary>
    /// Initialize the Player class.
    /// </summary>
    /// <param name="onObstacleCollision"></param>
    public void Initialize(Action onObstacleCollision)
    {
        Debug.Log($"{nameof(Player)} Initializing ...");

        ObstacleCollision += onObstacleCollision;
        _playerController.Initialize();

        Debug.Log($"{nameof(Player)} Initialized");
    }

    /// <summary>
    /// Start the player running. Called when the game starts.
    /// </summary>
    public void StartRunning()
    {
        _playerController.StartRunning();
    }

    /// <summary>
    /// Stop the player running. Called when the game is paused or ends.
    /// </summary>
    public void StopRunning()
    {
        _playerController.StopRunning();
    }

    /// <summary>
    /// Increase the player's speed. Called when the time phase changes.
    /// </summary>
    public void IncreaseSpeed()
    {
        _playerController.IncreaseSpeed();
    }

    /// <summary>
    /// Handle the player's collision with collectibles and obstacles.
    /// </summary>
    /// <param name="other"></param>
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
