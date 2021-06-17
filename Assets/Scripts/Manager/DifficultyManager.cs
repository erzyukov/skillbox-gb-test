using UnityEngine;

/// <summary>
/// Компонент управления сложностью игры
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public delegate void DifficultyAction(float difficulty);
    /// <summary>
    /// Событие срабатывающее при изменении сложности
    /// </summary>
    public event DifficultyAction OnChange;

    /// <summary>
    /// Текущая сложность игры
    /// </summary>
    public float Current { get; private set; } = 0;

    [SerializeField]
    [Tooltip("Кривая сложности зависящая от количества очков")]
    private AnimationCurve factor = default;

    [SerializeField]
    [Tooltip("Количество очков при которых сложность достигнет максимального значения")]
    private int maxDifficultyScore = 50;

    private void Start()
    {
        if (GameManager.instance != null && GameManager.instance.Score != default)
        {
            GameManager.instance.Score.OnChange += OnScoreChange;
        }
    }

    private void OnScoreChange(int score)
    {
        Current = factor.Evaluate(Mathf.Clamp01((float)score / (float)maxDifficultyScore));
        OnChange?.Invoke(Current);
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null && GameManager.instance.Score != default)
        {
            GameManager.instance.Score.OnChange -= OnScoreChange;
        }
    }

}
