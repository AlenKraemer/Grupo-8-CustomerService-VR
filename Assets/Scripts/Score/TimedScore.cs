using UnityEngine;
using System.Collections;

public class TimedScore : BasicScore
{
    [SerializeField] private float timeBetweenScoreChanges = 1.0f;
    [SerializeField] private int scoreChangeAmount = 1;
    [SerializeField] private bool autoStart = false;

    private Coroutine timedScoreCoroutine;
    private bool isRunning = false;

    private void Start()
    {
        if (autoStart)
        {
            StartTimedScore();
        }
    }

    public void StartTimedScore()
    {
        if (isRunning) return;

        isRunning = true;
        timedScoreCoroutine = StartCoroutine(TimedScoreRoutine());
    }

    public void StopTimedScore()
    {
        if (!isRunning) return;

        if (timedScoreCoroutine != null)
        {
            StopCoroutine(timedScoreCoroutine);
            timedScoreCoroutine = null;
        }

        isRunning = false;
    }

    public void SetTimeBetweenChanges(float newTime)
    {
        if (newTime <= 0)
        {
            Debug.LogWarning("Time between score changes must be greater than 0");
            return;
        }

        timeBetweenScoreChanges = newTime;
    }

    public void SetScoreChangeAmount(int amount)
    {
        scoreChangeAmount = amount; // Now allows negative values
    }

    private IEnumerator TimedScoreRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenScoreChanges);

            if (scoreChangeAmount >= 0)
                IncreaseScore(Mathf.Abs(scoreChangeAmount));
            else
                DecreaseScore(Mathf.Abs(scoreChangeAmount));
        }
    }

    public void ToggleTimedScore()
    {
        if (isRunning)
            StopTimedScore();
        else
            StartTimedScore();
    }

    private void OnDisable()
    {
        StopTimedScore();
    }
}