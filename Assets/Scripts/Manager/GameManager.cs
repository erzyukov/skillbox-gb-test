using UnityEngine;

/// <summary>
/// Игровой менеджер. Компонент-синглтон. Через него осуществляется управление остальными менеджерами.
/// </summary>
[RequireComponent (typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    /// <summary>
    /// Тип сцены
    /// </summary>
    [Tooltip("Тип текущей сцены")]
    public SceneType type;

    /// <summary>
    /// Состояние игры
    /// </summary>
    public GameState State { get; private set; } = default;

    /// <summary>
    /// Менеджер сцены
    /// </summary>
    public ScenesManager Scene { get; private set; } = default;

    /// <summary>
    /// Управление очками
    /// </summary>
    public GameScore Score { get; private set; } = default;

    /// <summary>
    /// Сложность игры
    /// </summary>
    public DifficultyManager Difficulty { get { return difficulty; } }

    /// <summary>
    /// Управление звуками
    /// </summary>
    public SoundManager Sound { get { return sound; } }

    /// <summary>
    /// Управление ресурсами уровня
    /// </summary>
    public StageResourceManager Resources { get { return resources; } }

    [SerializeField]
    [Tooltip("Компонент управления сложностью игры")]
    private DifficultyManager difficulty = default;

    [SerializeField]
    [Tooltip("Компонент управления звуками игры")]
    private SoundManager sound = default;

    [SerializeField]
    [Tooltip("Компонент управления ресурсами уровня")]
    private StageResourceManager resources = default;

    private void Awake()
    {
        if (instance == this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Scene = new ScenesManager();

        if (type == SceneType.Game)
        {
            Score = new GameScore();
            State = new GameState();
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}