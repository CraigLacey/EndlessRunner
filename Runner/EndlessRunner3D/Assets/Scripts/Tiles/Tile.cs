using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TriggerBox _triggerBox;

    public void Initialize(Action onTileExit)
    {
        if (_triggerBox != null)
        {
            _triggerBox.Initialize(onTileExit);
        }
    }
}
