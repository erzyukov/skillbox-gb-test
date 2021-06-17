using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент управления окном поражения
/// </summary>
public class DefeatWindow : Window
{
    [SerializeField]
    [Tooltip("Текстовое поле для отображения результата игры")]
    private Text result = default;

    public override void Open()
    {
        base.Open();

        string str = "Result: " + GameManager.instance.Score.Score + " \r\n";
        if (GameManager.instance.Score.IsBestResult)
        {
            str += "You are the BEST!";
        }
        else
        {
            str += "BEST: " + GameManager.instance.Score.BestScore;
        }

        result.text = str;
    }

    /// <summary>
    /// Закрывает окно и загружает главное меню
    /// </summary>
    public override void Close()
    {
        GameManager.instance.Scene.LoadMainMenu();
    }

    /// <summary>
    /// Обработка нажатия кнопки попробовать еще
    /// </summary>
    public void TryAgainButtonHandler()
    {
        GameManager.instance.Scene.RestatrLevel();
    }

}
