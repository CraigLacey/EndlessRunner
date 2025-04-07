using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    private bool _timerRunning = false;
    private float _startTime;
    private float _elapsedTime = 0f;

    private const float PHASE_DURATION = 10f;
    private float _phaseTimer = PHASE_DURATION;

    private UIManager _uiManager;
    
    public event Action TimerPhaseChange;

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
            _phaseTimer -= Time.deltaTime;
            if(_phaseTimer <= 0f)
            {
                TimerPhaseChange?.Invoke();
                _phaseTimer = PHASE_DURATION;
            }
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
        string timeText = string.Format("{0:000}m:{1:00}s:{2:000}ms", minutes, seconds, milliseconds);
        return timeText;
    }

    private void UpdateTimerText()
    {
        string timeText = GetTime();
        _timerText.text = $"Time: {timeText}";
    }
}
