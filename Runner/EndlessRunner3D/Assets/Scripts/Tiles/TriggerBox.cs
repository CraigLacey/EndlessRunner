using System;
using UnityEngine;

/// <summary>
/// Simple TriggerBox class that invokes an action when the player exits the trigger.
/// </summary>
public class TriggerBox : MonoBehaviour
{
    private Action PlayerExit;

    /// <summary>
    /// Initializes the TriggerBox with an action to be invoked when the player exits the trigger.
    /// </summary>
    /// <param name="onTileExit"></param>
    public void Initialize(Action onTileExit)
    {
        PlayerExit += onTileExit;
    }

    private void OnDestroy()
    {
        PlayerExit = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExit?.Invoke();
        }
    }
}
