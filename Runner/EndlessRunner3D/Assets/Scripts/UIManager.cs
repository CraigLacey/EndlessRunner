using TMPro;
using UnityEngine;

/// <summary>
/// UIManager is responsible for managing the UI elements in the game.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    /// <summary>
    /// Initializes the UIManager and registers it with the ServiceLocator.
    /// </summary>
    public void Initialize()
    {
        Debug.Log($"{nameof(UIManager)} -> Initializing");
        ServiceLocator.Register<UIManager>(this);
        Debug.Log($"{nameof(UIManager)} -> Initialized");
    }

    /// <summary>
    /// Gets the timer UI element.
    /// </summary>
    /// <returns></returns>
    public TextMeshProUGUI GetTimerUI()
    {
        return _timerText;
    }

    /// <summary>
    /// Gets the score UI element.
    /// </summary>
    /// <returns></returns>
    public TextMeshProUGUI GetScoreUI()
    {
        return _scoreText;
    }
}
