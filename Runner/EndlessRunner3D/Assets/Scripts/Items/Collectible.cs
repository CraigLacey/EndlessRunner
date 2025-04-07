using UnityEngine;

/// <summary>
/// Collectible is a class that represents a collectible item in the game.
/// </summary>
public class Collectible : MonoBehaviour
{
    [Header("Collectible Properties")]
    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceSpeed = 2f;

    private ScoreManager _scoreManager;
    private CollectibleManager _collectibleManager;

    /// <summary>
    /// Is the Collectible initialized?
    /// </summary>
    public bool Initialized => _initialized;
    private bool _initialized = false;

    /// <summary>
    /// Initialize the Collectible.
    /// </summary>
    public void Initialize()
    {
        _scoreManager = ServiceLocator.Get<ScoreManager>();
        _collectibleManager = ServiceLocator.Get<CollectibleManager>();
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }

        // Rotate the collectible
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

        // Bounce the collectible
        float newY = Mathf.Sin(Time.time * _bounceSpeed) * _bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    /// <summary>
    /// Collect the collectible and recycle it back to the Collectible pool.
    /// </summary>
    public void Collect()
    {
        if (!_initialized)
        {
            return;
        }

        // Add the collectible value to the player's score
        Debug.Log($"{nameof(Collectible)} -> Collected!");
        _scoreManager.UpdateScore(_scoreValue);
        _collectibleManager.RecycleCollectible(gameObject);
    }
}
