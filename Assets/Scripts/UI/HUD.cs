using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент отображения данных состояния игры
/// </summary>
public class HUD : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Текстовое поле для отображения очков")]
    private Text score = default;

    [SerializeField]
    [Tooltip("Текстовое поле для отображения лучшего результата")]
    private Text bestScore = default;

    private void Start()
    {
        if (GameManager.instance.Score != default)
        {
            GameManager.instance.Score.OnChange += OnScoreChange;
            SetScore(GameManager.instance.Score.Score.ToString());
            SetBestScore(GameManager.instance.Score.BestScore.ToString());
        }
    }

    private void SetScore(string value)
    {
        score.text = value;
    }

    private void SetBestScore(string value)
    {
        bestScore.text = "Best " + value;
    }

    private void OnScoreChange(int amount)
    {
        SetScore(amount.ToString());
    }

    private void OnDestroy()
    {
        if (GameManager.instance != default && GameManager.instance.Score != default)
        {
            GameManager.instance.Score.OnChange -= OnScoreChange;
        }
    }
}
