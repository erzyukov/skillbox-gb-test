using UnityEngine;

/// <summary>
/// Менеджер управления пользовательским интерфейсом
/// </summary>
public class UIManager : MonoBehaviour
{
    public SceneType type = SceneType.Game;

    [HideInInspector]
    [Tooltip("Окно титров")]
    public bool creditsWindowEnabled = false;

    [HideInInspector]
    [Tooltip("Окно победы")]
    public bool winWindowEnabled = false;

    [HideInInspector]
    [Tooltip("Окно окончания игры")]
    public bool gameOverWindowEnabled = false;

    [HideInInspector]
    [Tooltip("Окно помощи")]
    public bool helpWindowEnabled = false;

    [SerializeField]
    [Tooltip("Окно опций")]
    private bool optionsWindowEnabled = false;

    [SerializeField]
    [Tooltip("Прятать кнопку выхода (для WebGL версии)")]
    private bool isHideQuitButton = false;

    private DefeatWindow defeat;

    private void Awake()
    {
        GameObject ui = new GameObject("UI");

        switch(type)
        {
            case SceneType.MainMenu:
                InitMainMenuUI(ui.transform);
                break;
            case SceneType.Game:
                InitGameUI(ui.transform);
                break;
        }
    }

    private void Start()
    {
        if (GameManager.instance.State != default)
        {
            GameManager.instance.State.OnChange += OnGameStateChange;
        }
    }

    /// <summary>
    /// Инициализация интерфейса сцены с главным меню
    /// </summary>
    /// <param name="parent">Ссылка на родительский объект для UI</param>
    private void InitMainMenuUI(Transform parent)
    {
        Instantiate(Resources.Load<GameObject>("UI/EventSystem"), parent);

        GameObject mainMenuObject = Instantiate(Resources.Load<GameObject>("UI/MainMenu"), parent);
        UIMainMenu mainMenu = mainMenuObject.GetComponent<UIMainMenu>();

        if (optionsWindowEnabled)
        {
            OptionsWindow options = InitWindow<OptionsWindow>("Options", parent);
            mainMenu.OptionsWindow = options;
        }

        if (creditsWindowEnabled)
        {
            CreditsWindow credits = InitWindow<CreditsWindow>("Credits", parent);
            mainMenu.CreditsWindow = credits;

        }

        ConfirmWindow confirm = InitWindow<ConfirmWindow>("Confirm", parent);
        mainMenu.ConfirmWindow = confirm;

        mainMenu.IsHideQuitButton = isHideQuitButton;
    }

    /// <summary>
    /// Инициализация интерфейса игровой сцены
    /// </summary>
    /// <param name="parent">Ссылка на родительский объект для UI</param>
    private void InitGameUI(Transform parent)
    {
        Instantiate(Resources.Load<GameObject>("UI/EventSystem"), parent);

        PauseWindow pause = InitWindow<PauseWindow>("Pause", parent);
        if (optionsWindowEnabled)
        {
            OptionsWindow options = InitWindow<OptionsWindow>("Options", parent);
            pause.OptionsWindow = options;
        }

        if (helpWindowEnabled)
        {
            HelpWindow help = InitWindow<HelpWindow>("Help", parent);
            pause.HelpWindow = help;
        }

        defeat = InitWindow<DefeatWindow>("Defeat", parent);

        ConfirmWindow confirm = InitWindow<ConfirmWindow>("Confirm", parent);
        pause.ConfirmWindow = confirm;

        pause.IsHideQuitButton = isHideQuitButton;

        GameObject hudObject = Instantiate(Resources.Load<GameObject>("UI/HUD"), parent);
        hudObject.GetComponent<HUD>();
    }

    /// <summary>
    /// Инициализация окна
    /// </summary>
    /// <typeparam name="T">Название класса окна унаследованного от Window</typeparam>
    /// <param name="name">Название префаба окна, лежащего в папке "/Resources/UI/Windows/"</param>
    /// <param name="parent">Родительский объект</param>
    /// <returns></returns>
    private T InitWindow<T>(string name, Transform parent) where T : Window
    {
        GameObject prefab = Resources.Load<GameObject>("UI/Windows/" + name);
        if (prefab)
        {
            GameObject windowObject = Instantiate<GameObject>(prefab, parent);
            return windowObject.GetComponent<T>();
        }

        return default;
    }

    private void OnGameStateChange(GameStateType type)
    {
        switch (type)
        {
            case GameStateType.GameOver:
                defeat.Open();
                break;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null && GameManager.instance.State != default)
        {
            GameManager.instance.State.OnChange -= OnGameStateChange;
        }
    }

}
