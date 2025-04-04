using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    private bool _timerRunning = false;
    private float _startTime;

    public void Initialize()
    {
        Debug.Log($"{nameof(TimerManager)} -> Initializing");

        Debug.Log($"{nameof(TimerManager)} -> Initialized");
    }

    private void Update()
    {
        if (_timerRunning)
        {
            float elapsedTime = Time.time - _startTime;
            UpdateTimerText(elapsedTime);
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

    private void UpdateTimerText(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
        _timerText.text = string.Format("Time: {0:000}m:{1:00}s:{2:000}ms", minutes, seconds, milliseconds);
    }
}
