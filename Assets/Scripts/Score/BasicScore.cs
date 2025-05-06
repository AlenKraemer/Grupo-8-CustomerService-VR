using UnityEngine;

public class BasicScore : MonoBehaviour
{
    [SerializeField] private int currentScore = 0;
    [SerializeField] private float scoreMultiplier = 1f;
    private int minScore = int.MinValue;
    private int maxScore = int.MaxValue;

    public int CurrentScore => currentScore;
    public float ScoreMultiplier => scoreMultiplier;

    public virtual void IncreaseScore(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Trying to increase score with negative value. Use DecreaseScore instead.");
            return;
        }

        int multipliedAmount = Mathf.RoundToInt(amount * scoreMultiplier);
        int newScore = Mathf.Clamp(currentScore + multipliedAmount, minScore, maxScore);
        if (newScore != currentScore)
        {
            currentScore = newScore;
        }
    }

    public virtual void DecreaseScore(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Trying to decrease score with negative value. Use IncreaseScore instead.");
            return;
        }

        int multipliedAmount = Mathf.RoundToInt(amount * scoreMultiplier);
        int newScore = Mathf.Clamp(currentScore - multipliedAmount, minScore, maxScore);
        if (newScore != currentScore)
        {
            currentScore = newScore;
        }
    }

    public virtual void ResetScore()
    {
        currentScore = 0;
    }

    public virtual void SetScore(int newScore)
    {
        int clampedScore = Mathf.Clamp(newScore, minScore, maxScore);
        if (clampedScore != currentScore)
        {
            currentScore = clampedScore;
        }
    }

    public virtual void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = Mathf.Max(0, multiplier);
    }

    public virtual void ResetScoreMultiplier()
    {
        scoreMultiplier = 1f;
    }
}