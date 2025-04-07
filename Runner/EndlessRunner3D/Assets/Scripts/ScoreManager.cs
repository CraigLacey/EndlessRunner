using TMPro;
using UnityEngine;

/// <summary>
/// ScoreManager is responsible for managing the score in the game.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private UIManager _uiManager;
    private int _currentScore = 0;

    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int CurrentScore => _currentScore;

    /// <summary>
    /// Initializes the ScoreManager
    /// </summary>
    public void Initialize()
    {
        Debug.Log($"{nameof(ScoreManager)} -> Initializing");
        _uiManager = ServiceLocator.Get<UIManager>();
        _scoreText = _uiManager.GetScoreUI();
        _currentScore = 0;
        UpdateScore(0);
        Debug.Log($"{nameof(ScoreManager)} -> Initialized");
    }

    /// <summary>
    /// Updates the score by a given amount.
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        _currentScore += score;
        _scoreText.text = $"Score: {_currentScore}"; 
    }
}
