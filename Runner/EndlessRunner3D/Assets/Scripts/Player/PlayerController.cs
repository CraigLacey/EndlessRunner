using UnityEngine;

/// <summary>
/// PlayerController class is responsible for controlling the player's movement.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Stats")]
    [SerializeField] private PlayerStatsSO _playerStats;

    // This variable is used to track the player's horizontal position
    // 0 = center
    // -1 = left
    // 1 = right
    private int _horizontalPosition = 0;
    private float _leftPosX = -2.5f;
    private float _rightPosX = 2.5f;

    private Rigidbody _rb;
    private bool _isRunning = false;

    // Movement stats - populated from PlayerStatsSO on init
    private float _moveSpeed = 8f;
    private float _lateralMoveSpeed = 10f;
    private float _moveSpeedMultiplier = 1f;
    private float _multiplierIncreaseRate = 0.1f;

    /// <summary>
    /// Initialize the PlayerController.
    /// </summary>
    public void Initialize()
    {
        Debug.Log($"{nameof(PlayerController)} Initializing ...");

        _rb = GetComponent<Rigidbody>();

        // Set the player's initial stats
        _moveSpeed = _playerStats.MoveSpeed;
        _lateralMoveSpeed = _playerStats.LateralMoveSpeed;
        _moveSpeedMultiplier = _playerStats.MoveSpeedMultiplier;
        _multiplierIncreaseRate = _playerStats.MultiplierIncreaseRate;

        Debug.Log($"{nameof(PlayerController)} Initialized");
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            }
        }
    }

    /// <summary>
    /// Move the player to the left if possible
    /// </summary>
    public void MoveLeft()
    {
        _horizontalPosition -= 1;
        if (_horizontalPosition < -1)
        {
            _horizontalPosition = -1;
        }
    }

    /// <summary>
    /// Move the player to the right if possible
    /// </summary>
    public void MoveRight()
    {
        _horizontalPosition += 1;
        if (_horizontalPosition > 1)
        {
            _horizontalPosition = 1;
        }
    }

    /// <summary>
    /// Start the player running. This is called when the game starts.
    /// </summary>
    public void StartRunning()
    {
        _isRunning = true;
    }

    /// <summary>
    /// Stop the player running. This is called when the game is paused or ends.
    /// </summary>
    public void StopRunning()
    {
        _isRunning = false;
    }

    /// <summary>
    /// Increase the player's speed. This is called when the time phase changes.
    /// </summary>
    public void IncreaseSpeed()
    {
        _moveSpeedMultiplier += _multiplierIncreaseRate;
        if(_moveSpeedMultiplier > _playerStats.MaxMultiplier)
        {
            _moveSpeedMultiplier = _playerStats.MaxMultiplier;
        }
    }

    /// <summary>
    /// Player's physics based movement is updated in FixedUpdate.
    /// </summary>
    private void FixedUpdate()
    {
        if (!_isRunning)
        {
            return;
        }

        // Forward movement (automatic)
        Vector3 forwardMovement = Vector3.forward * (_moveSpeed * _moveSpeedMultiplier) * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + forwardMovement);

        // Update Horizontal Position.
        if (_horizontalPosition == -1)
        {
            Vector3 newPos = new Vector3(_leftPosX, _rb.position.y, _rb.position.z);
            _rb.position = Vector3.Lerp(_rb.position, newPos, Time.fixedDeltaTime * _lateralMoveSpeed);
        }
        else if (_horizontalPosition == 1)
        {
            Vector3 newPos = new Vector3(_rightPosX, _rb.position.y, _rb.position.z);
            _rb.position = Vector3.Lerp(_rb.position, newPos, Time.fixedDeltaTime * _lateralMoveSpeed);
        }
        else
        {
            Vector3 newPos = new Vector3(0, _rb.position.y, _rb.position.z);
            _rb.position = Vector3.Lerp(_rb.position, newPos, Time.fixedDeltaTime * _lateralMoveSpeed);
        }
    }
}
