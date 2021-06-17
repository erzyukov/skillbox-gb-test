using UnityEngine;

/// <summary>
/// Компонент управления загрузкой уровня
/// </summary>
public class StageResourceManager : MonoBehaviour
{
    public GameObject Player { get { return player; } }

    [SerializeField]
    [Tooltip("Название префаба уровня (папка Resourses/Stages)")]
    private string stagePrefabName = "StageOne";

    [SerializeField]
    [Tooltip("Название префаба игрока (папка Resourses)")]
    private string playerPrefabName = "Player";

    [SerializeField]
    [Tooltip("Название название главной музыкальной темы уровня (папка Resourses/Audio)")]
    private string themeName = "Theme";

    private GameObject player;

    private void Awake()
    {
        // инициализируем уровень
        GameObject currentStage = Instantiate(Resources.Load<GameObject>("Stages/" + stagePrefabName));
        // получаем доступ к компоненту настроек уровня
        StageSettings settings = currentStage.GetComponent<StageSettings>();

        // инициализируем игрока
        player = Instantiate(Resources.Load<GameObject>(playerPrefabName));
        // простовляем стартовую позицию
        player.transform.position = settings.PlayerSpawnPoint.position;
    }

    private void Start()
    {
        AudioSource theme = GameManager.instance.Sound.InitAudioSource(gameObject, themeName, true, 0.1f, SoundManager.group.Music);
        theme.Play();
    }

}