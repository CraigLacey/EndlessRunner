using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _lateralMoveSpeed = 10f;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    // This variable is used to track the player's horizontal position
    // 0 = center
    // -1 = left
    // 1 = right
    private int _horizontalPosition = 0;
    private float _leftPosX = -2.5f;
    private float _rightPosX = 2.5f;

    private Rigidbody _rb;
    private bool _isRunning = false;

    public void Initialize()
    {
        Debug.Log($"{nameof(PlayerController)} Initializing ...");

        _rb = GetComponent<Rigidbody>();
        _leftButton.onClick.AddListener(MoveLeft);
        _rightButton.onClick.AddListener(MoveRight);

        Debug.Log($"{nameof(PlayerController)} Initialized");
    }

    public void MoveLeft()
    {
        _horizontalPosition -= 1;
        if (_horizontalPosition < -1)
        {
            _horizontalPosition = -1;
        }
    }

    public void MoveRight()
    {
        _horizontalPosition += 1;
        if (_horizontalPosition > 1)
        {
            _horizontalPosition = 1;
        }
    }

    internal void StartRunning()
    {
        _isRunning = true;
    }

    internal void StopRunning()
    {
        _isRunning = false;
    }

    private void FixedUpdate()
    {
        if (!_isRunning)
        {
            return;
        }

        // Forward movement (automatic)
        Vector3 forwardMovement = Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
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
            _rb.position = Vector3.Lerp(_rb.position,newPos, Time.fixedDeltaTime * _lateralMoveSpeed);
        }
        else
        {
            Vector3 newPos = new Vector3(0, _rb.position.y, _rb.position.z);
            _rb.position = Vector3.Lerp(_rb.position, newPos, Time.fixedDeltaTime * _lateralMoveSpeed);
        }
    }
}
