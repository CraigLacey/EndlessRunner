using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    public void Initialize()
    {
        Debug.Log($"{nameof(Player)} Initializing ...");

        _playerController.Initialize();

        Debug.Log($"{nameof(Player)} Initialized");
    }
}
