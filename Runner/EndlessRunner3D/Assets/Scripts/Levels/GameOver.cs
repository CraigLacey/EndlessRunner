using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerValue;
    [SerializeField] private TextMeshProUGUI _scoreValue;

    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _timerValue.text = ServiceLocator.Get<TimerManager>().GetTime();
        _scoreValue.text = ServiceLocator.Get<ScoreManager>().GetScore().ToString();
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
