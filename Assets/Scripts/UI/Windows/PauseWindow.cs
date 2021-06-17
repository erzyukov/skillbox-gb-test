using UnityEngine;

/// <summary>
/// Компонент управления окном паузы
/// </summary>
public class PauseWindow : Window
{
    /// <summary>
    /// Ссылка на объект окна настроек
    /// </summary>
    public OptionsWindow OptionsWindow { private get; set; } = default;


    /// <summary>
    /// Прятать кнопку выхода
    /// </summary>
    public bool IsHideQuitButton { private get; set; } = false;

    /// <summary>
    /// Ссылка на объект окна подтверждения
    /// </summary>
    public ConfirmWindow ConfirmWindow { private get; set; } = default;

    /// <summary>
    /// Ссылка на объект окна помощи
    /// </summary>
    public HelpWindow HelpWindow { private get; set; } = default;

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
    /// Ссылка на объект кнопки помощи
    /// </summary>
    [SerializeField]
    private GameObject helpButton = default;

    private void Start()
    {
        // если не задано окно настроек - прячем кнопку
        if (optionsButton && OptionsWindow == default)
        {
            optionsButton.SetActive(false);
        }

        // если не задано окно помощи - прячем кнопку
        if (helpButton && HelpWindow == default)
        {
            helpButton.SetActive(false);
        }

        // скрываем кнопку выхода, если она нам не нужна
        if (quitButton && IsHideQuitButton)
        {
            quitButton.SetActive(false);
        }
    }

    void Update()
    {
        if (!GameManager.instance.State.IsPauseAllow())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.State.State != GameStateType.Pause)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    /// <summary>
    /// Открывает окно паузы и включает паузу
    /// </summary>
    public override void Open()
    {
        base.Open();
        GameManager.instance.State.PauseGame();
    }

    /// <summary>
    /// Закрывает окно паузы и выключает паузу
    /// Вместе с окном паузы, закрываются все зависимые окна
    /// </summary>
    public override void Close()
    {
        base.Close();
        if (OptionsWindow != default)
        {
            OptionsWindow.Close();
        }
        if (HelpWindow != default)
        {
            HelpWindow.Close();
        }
        GameManager.instance.State.ResumeGame();
    }

    /// <summary>
    /// Обработка нажатия кнопки помощи
    /// </summary>
    public void HelpButtonHandler()
    {
        if (OptionsWindow != default)
        {
            OptionsWindow.Close();
        }
        if (HelpWindow != default)
        {
            HelpWindow.Open();
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки настройки
    /// </summary>
    public void OptionsButtonHandler()
    {
        if (OptionsWindow != default)
        {
            OptionsWindow.Open();
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки рестарт
    /// </summary>
    public void RestartButtonHandler()
    {
        GameManager.instance.Scene.RestatrLevel();
    }

    /// <summary>
    /// Обработка нажатия кнопки главное меню
    /// </summary>
    public void MainMenuButtonHandler()
    {
        ConfirmWindow.Open();
        ConfirmWindow.SetYesAction(() => {
            GameManager.instance.Scene.LoadMainMenu();
        });
        ConfirmWindow.SetNoAction(() => {
            ConfirmWindow.Close();
        });
    }

    /// <summary>
    /// Обработка нажатия кнопки выход
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

}
