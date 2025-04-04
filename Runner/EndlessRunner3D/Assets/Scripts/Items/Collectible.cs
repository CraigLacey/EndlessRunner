using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceSpeed = 2f;

    private ScoreManager _scoreManager;
    private bool _initialized = false;

    private void Awake()
    {
        _scoreManager = ServiceLocator.Get<ScoreManager>();
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

    public void Collect()
    {
        if (!_initialized)
        {
            return;
        }

        // Add the collectible to the player's inventory or score
        Debug.Log($"{nameof(Collectible)} -> Collected!");
        _scoreManager.UpdateScore(_scoreValue);

        // Destroy the collectible object
        Destroy(gameObject);
    }
}
