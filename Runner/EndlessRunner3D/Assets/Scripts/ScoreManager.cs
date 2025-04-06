using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore = 0;

    public void Initialize()
    {
        Debug.Log($"{nameof(ScoreManager)} -> Initializing");
        _scoreText.text = "0";
        Debug.Log($"{nameof(ScoreManager)} -> Initialized");
    }

    public void UpdateScore(int score)
    {
        _currentScore += score;
        _scoreText.text = $"Score: {_currentScore}"; 
    }

    public int GetScore()
    {
        return _currentScore;
    }
}
