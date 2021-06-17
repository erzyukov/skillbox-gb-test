using UnityEngine;

/// <summary>
/// Класс ведущий счет очков набраных игроком
/// </summary>
public class GameScore
{
    public delegate void GameScoreAction(int score);
    /// <summary>
    /// Событие срабатывающее при изменении количества очков
    /// </summary>
    public event GameScoreAction OnChange;

    /// <summary>
    /// Текущее количество очков
    /// </summary>
    public int Score { get; private set; } = 0;

    /// <summary>
    /// Лучший результат
    /// </summary>
    public int BestScore { get; private set; } = 0;

    /// <summary>
    /// Является ли текущий результат лучшим
    /// </summary>
    public bool IsBestResult { get; private set; } = false;

    public GameScore()
    {
        UpdateScore(0);
        BestScore = PlayerPrefs.GetInt("BestScore");
    }

    /// <summary>
    /// Повышает текущие очки на указанное количество
    /// </summary>
    /// <param name="amount">Количество очков</param>
    public void Increase(int amount)
    {
        UpdateScore(Score + amount);
    }

    /// <summary>
    /// Обновляет текущее количество очков, при этом вызывает событие оповещающее о смене количества очков
    /// </summary>
    /// <param name="value">Новое значение</param>
    private void UpdateScore(int value)
    {
        Score = value;
        OnChange?.Invoke(value);

        if (Score > BestScore)
        {
            UpdateBestScore(Score);
        }
    }

    /// <summary>
    /// Обновляет лучший результат
    /// </summary>
    /// <param name="value">Количество очков</param>
    public void UpdateBestScore(int value)
    {
        IsBestResult = true;
        BestScore = value;
        PlayerPrefs.SetInt("BestScore", value);
    }
}