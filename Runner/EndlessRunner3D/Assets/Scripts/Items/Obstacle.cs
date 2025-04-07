using UnityEngine;

/// <summary>
/// Obstacle is a class that represents an obstacle in the game.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Properties")]
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceSpeed = 2f;

    /// <summary>
    /// Is the Obstacle initialized?
    /// </summary>
    public bool Initialized => _initialized;
    private bool _initialized = false;

    /// <summary>
    /// Initialize the Obstacle.
    /// </summary>
    public void Initialize()
    {
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }

        // Bounce the obstacle
        float newY = Mathf.Sin(Time.time * _bounceSpeed) * _bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
