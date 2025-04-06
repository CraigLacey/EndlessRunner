using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceSpeed = 2f;

    public bool Initialized => _initialized;
    private bool _initialized = false;

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
