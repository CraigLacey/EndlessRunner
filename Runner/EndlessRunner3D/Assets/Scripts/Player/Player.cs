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

    internal void StartRunning()
    {
        _playerController.StartRunning();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log($"{nameof(Player)} -> Collected an item!");
            other.GetComponent<Collectible>().Collect();
        }
    }
}
