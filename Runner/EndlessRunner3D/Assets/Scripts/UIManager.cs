using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Initialize()
    {
        Debug.Log($"{nameof(UIManager)} -> Initializing");
        ServiceLocator.Register<UIManager>(this);
        Debug.Log($"{nameof(UIManager)} -> Initialized");
    }

    public TextMeshProUGUI GetTimerUI()
    {
        return _timerText;
    }

    public TextMeshProUGUI GetScoreUI()
    {
        return _scoreText;
    }
}
