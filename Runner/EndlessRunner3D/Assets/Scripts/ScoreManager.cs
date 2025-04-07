using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private UIManager _uiManager;
    private int _currentScore = 0;

    public int CurrentScore => _currentScore;

    public void Initialize()
    {
        Debug.Log($"{nameof(ScoreManager)} -> Initializing");
        _uiManager = ServiceLocator.Get<UIManager>();
        _scoreText = _uiManager.GetScoreUI();
        _currentScore = 0;
        UpdateScore(0);
        Debug.Log($"{nameof(ScoreManager)} -> Initialized");
    }

    public void UpdateScore(int score)
    {
        _currentScore += score;
        _scoreText.text = $"Score: {_currentScore}"; 
    }
}
