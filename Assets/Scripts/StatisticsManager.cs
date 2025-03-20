using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;
    public int correctAnswersCounter;//for quiz
    public int wrongAnswersCounter;//for quiz

    public int mistakeCounter;//count for mistake when placing component
    [SerializeField] private float timer , currentTimer;
    [SerializeField] private bool isTimerStart;
    [Header("Countdown start waitTime")]
    [SerializeField] private bool isCountdownStart, canTimerStart;
    [SerializeField] private float countDown,currentCountdown;

    public int hours, minutes, seconds;
    public bool IsTimerStart { get => isTimerStart; set => isTimerStart = value; }
    public bool IsCountdownStart { get => isCountdownStart; set => isCountdownStart = value; }
    public float CountDown { get => countDown; set => countDown = value; }
    public bool CanTimerStart { get => canTimerStart; set => canTimerStart = value; }
    public float Timer { get => timer; set => timer = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (IsCountdownStart)
        {
            CountDown -= Time.deltaTime;

            if (CountDown <= 0)
            {
                isCountdownStart = false;
                canTimerStart = true;
            }
            
        }

        if (isTimerStart)
        {
            Timer += Time.deltaTime;
            FormatTime();
        }
       
    }

    public void FormatTime()
    {
         hours = Mathf.FloorToInt(Timer/3600f);
         minutes = Mathf.FloorToInt((Timer % 3600) /60);
         seconds = Mathf.FloorToInt(Timer % 60);
    }

    public string FormattedTime()
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}",hours,minutes,seconds);
    }
    public void CountCorrectAnswer()
    {
        correctAnswersCounter++;
        Debug.Log($"correct answer count:{correctAnswersCounter}");
    }

    public void CountWrongAnswer()
    {
        wrongAnswersCounter++;
        Debug.Log($"wrong answer count:{wrongAnswersCounter}");
    }

    public void CountPlaceMistake()
    {
        mistakeCounter++;
    }
}
