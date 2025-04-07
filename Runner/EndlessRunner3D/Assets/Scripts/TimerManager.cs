using System.Text;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    private bool _timerRunning = false;
    private float _startTime;
    private float _elapsedTime = 0f;

    private UIManager _uiManager;
    private string _timeTextValue;
    private StringBuilder _stringBuilder = new();

    public void Initialize()
    {
        Debug.Log($"{nameof(TimerManager)} -> Initializing");
        _uiManager = ServiceLocator.Get<UIManager>();
        _timerText = _uiManager.GetTimerUI();
        _elapsedTime = 0f;
        UpdateTimerText();
        Debug.Log($"{nameof(TimerManager)} -> Initialized");
    }

    private void Update()
    {
        if (_timerRunning)
        {
            _elapsedTime = Time.time - _startTime;
            UpdateTimerText();
        }
    }

    public void StartTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Starting Timer");
        _timerRunning = true;
        _startTime = Time.time;
    }

    public void PauseTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Pausing Timer");
        _timerRunning = false;
    }

    public void ResumeTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Resuming Timer");
        _timerRunning = true;
    }

    public string GetTime()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((_elapsedTime * 1000) % 1000);

        _stringBuilder.Clear();
        _stringBuilder.Append(minutes.ToString("000"));
        _stringBuilder.Append("m:");
        _stringBuilder.Append(seconds.ToString("00"));
        _stringBuilder.Append("s:");
        _stringBuilder.Append(milliseconds.ToString("000"));
        _stringBuilder.Append("ms");

        _timeTextValue = _stringBuilder.ToString();
        return _timeTextValue;
    }

    private void UpdateTimerText()
    {
        _timerText.text = $"Time: {_timeTextValue}";
    }
}
