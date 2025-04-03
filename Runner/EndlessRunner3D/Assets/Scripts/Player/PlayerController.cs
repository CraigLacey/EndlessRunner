using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 7f;

    private Rigidbody _rb;
    private bool _initialized = false;
    private float _horizontalInput;

    public void Initialize()
    {
        Debug.Log($"{nameof(PlayerController)} Initializing ...");

        _rb = GetComponent<Rigidbody>();
        _initialized = true;

        Debug.Log($"{nameof(PlayerController)} Initialized");
    }

    void Update()
    {
        if (!_initialized) { return; }

        // Get horizontal input from touch
        _horizontalInput = 0f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
            {
                _horizontalInput = -1f; // Move left
            }
            else
            {
                _horizontalInput = 1f; // Move right
            }
        }
    }

    void FixedUpdate()
    {
        // Forward movement (automatic)
        Vector3 forwardMovement = Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + forwardMovement);

        // Horizontal movement (physics-based)
        Vector3 horizontalMovement = Vector3.right * _horizontalInput * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + horizontalMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    _isGrounded = true;
        //}
        //else if (collision.gameObject.CompareTag("Obstacle"))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }
}
