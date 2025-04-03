using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        Debug.Log($"{nameof(Gameplay)} -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Gameplay)} -> Initializing");

        _player.Initialize();

        Debug.Log($"{nameof(Gameplay)} -> Initialized");
    }
}
