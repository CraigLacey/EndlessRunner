using System.Text;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the timer for the game.
/// </summary>
public class TimerManager : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    private bool _timerRunning = false;
    private float _startTime;
    private float _elapsedTime = 0f;

    // Gameplay Phase Progression
    private GameProgessionSO _gameProgessionData;
    private float _phaseDuration;
    private float _phaseTimer;

    private UIManager _uiManager;
    private string _timeTextValue;
    private StringBuilder _stringBuilder = new();

    /// <summary>
    /// Event triggered when the timer phase changes.
    /// </summary>
    public event Action TimerPhaseChange;

    /// <summary>
    /// Initializes the TimerManager
    /// </summary>
    public void Initialize()
    {
        Debug.Log($"{nameof(TimerManager)} -> Initializing");
        _gameProgessionData = ServiceLocator.Get<Gameplay>().ProgessionData;
        _phaseDuration = _gameProgessionData.PhaseDurationSeconds;
        _phaseTimer = _phaseDuration;

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
                _phaseTimer = _phaseDuration;
            }
            UpdateTimerText();
        }
    }

    /// <summary>
    /// Starts the timer. Called when the game starts.
    /// </summary>
    public void StartTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Starting Timer");
        _timerRunning = true;
        _startTime = Time.time;
    }

    /// <summary>
    /// Pauses the timer. Called when the game is paused.
    /// </summary>
    public void PauseTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Pausing Timer");
        _timerRunning = false;
    }

    /// <summary>
    /// Resumes the timer. Called when the game is resumed.
    /// </summary>
    public void ResumeTimer()
    {
        Debug.Log($"{nameof(TimerManager)} -> Resuming Timer");
        _timerRunning = true;
    }

    /// <summary>
    /// Returns the elapsed time in a formatted string.
    /// </summary>
    /// <returns></returns>
    public string GetTime()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((_elapsedTime * 1000) % 1000);

        _stringBuilder.Clear();
        _stringBuilder.Append(minutes.ToString("00"));
        _stringBuilder.Append("m:");
        _stringBuilder.Append(seconds.ToString("00"));
        _stringBuilder.Append("s:");
        _stringBuilder.Append(milliseconds.ToString("00"));
        _stringBuilder.Append("ms");

        _timeTextValue = _stringBuilder.ToString();
        return _timeTextValue;
    }

    /// <summary>
    /// Updates the timer text in the UI.
    /// </summary>
    private void UpdateTimerText()
    {
        _timerText.text = $"Time: {GetTime()}";
    }
}
