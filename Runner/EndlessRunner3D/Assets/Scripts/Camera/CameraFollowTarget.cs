using UnityEngine;

/// <summary>
/// CameraFollowTarget is responsible for making the camera follow a target object.
/// </summary>
public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
    }
}
