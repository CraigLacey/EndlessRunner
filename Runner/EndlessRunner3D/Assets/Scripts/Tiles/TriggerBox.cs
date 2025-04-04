using System;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    private Action PlayerExit;

    internal void Initialize(Action onTileExit)
    {
        PlayerExit += onTileExit;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExit?.Invoke();
        }
    }
}
