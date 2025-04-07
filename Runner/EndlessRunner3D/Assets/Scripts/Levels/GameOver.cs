using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameOver is responsible for displaying the game over screen.
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI _timerValue;
    [SerializeField] private TextMeshProUGUI _scoreValue;

    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _timerValue.text = ServiceLocator.Get<TimerManager>().GetTime();
        _scoreValue.text = ServiceLocator.Get<ScoreManager>().CurrentScore.ToString();
        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDestroy()
    {
        _retryButton.onClick.RemoveListener(OnRetryButtonClicked);
        _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnRetryButtonClicked()
    {
        // Scene 0 will always be the Application Start scene.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
