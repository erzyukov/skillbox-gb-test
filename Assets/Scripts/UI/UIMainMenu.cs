using UnityEngine;

/// <summary>
/// Компонент главного меню
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    /// <summary>
    /// Ссылка на объект окна настроек
    /// </summary>
    public OptionsWindow OptionsWindow { private get; set; } = default;

    /// <summary>
    /// Ссылка на объект окна титров
    /// </summary>
    public CreditsWindow CreditsWindow { private get; set; } = default;

    /// <summary>
    /// Ссылка на объект окна подтверждения
    /// </summary>
    public ConfirmWindow ConfirmWindow { private get; set; } = default;

    /// <summary>
    /// Прятать кнопку выхода
    /// </summary>
    public bool IsHideQuitButton { private get; set; } = false;

    /// <summary>
    /// Ссылка на объект кнопки выхода
    /// </summary>
    [SerializeField]
    private GameObject quitButton = default;

    /// <summary>
    /// Ссылка на объект кнопки настроек
    /// </summary>
    [SerializeField]
    private GameObject optionsButton = default;

    /// <summary>
    /// Ссылка на объект кнопки титров
    /// </summary>
    [SerializeField]
    private GameObject creditsButton = default;

    private void Start()
    {
        // если не задано окно настроек - прячем кнопку
        if (optionsButton && OptionsWindow == default)
        {
            optionsButton.SetActive(false);
        }
        // если не задано окно титров - прячем кнопку
        if (creditsButton && CreditsWindow == default)
        {
            creditsButton.SetActive(false);
        }
        // скрываем кнопку выхода, если она нам не нужна
        if (quitButton && IsHideQuitButton)
        {
            quitButton.SetActive(false);
        }
        gameObject.AddComponent<ButtonEventsHandler>();
    }

    /// <summary>
    /// Обработка нажатия кнопки Старт
    /// </summary>
    public void StartButtonHandler()
    {
        GameManager.instance.Scene.LoadNextLevel();
    }

    /// <summary>
    /// Обработка нажатия кнопки Настройки
    /// </summary>
    public void OptionButtonHandler()
    {
        if (OptionsWindow != default)
        {
            OptionsWindow.Open();
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки Выход
    /// </summary>
    public void QuitButtonHandler()
    {
        ConfirmWindow.Open();
        ConfirmWindow.SetYesAction(() => {
            GameManager.instance.Scene.QuitGame();
        });
        ConfirmWindow.SetNoAction(() => {
            ConfirmWindow.Close();
        });
    }

    /// <summary>
    /// Обработка нажатия кнопки Титры
    /// </summary>
    public void CreditsButtonHandler()
    {
        if (CreditsWindow != default)
        {
            CreditsWindow.Open();
        }
    }

}
