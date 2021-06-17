using UnityEngine;

/// <summary>
/// Компонент спаунит привидений на сцене в зависимости от сложности
/// </summary>
public class GhostSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Места для спауна приведений")]
    private Transform[] spawnPoints = default;

    [SerializeField]
    [Tooltip("Максимальное количество приведений")]
    private byte maxCount = 3;

    [SerializeField]
    [Tooltip("Промежуток сложности, по достижении которого будет появляться новое привидение")]
    [Range(0f, 1f)]
    private float difficultySpawnInterval = 0.25f;

    [SerializeField]
    [Tooltip("Спаунер подарков")]
    private GiftSpawner spawner = default;

    private GhostController[] ghosts;
    private byte indexOnScene = 0;

    private AudioSource spawnAudio = null;

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>("Core/Ghost");

        ghosts = new GhostController[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            GameObject obj = Instantiate<GameObject>(prefab);
            ghosts[i] = obj.GetComponent<GhostController>();
        }
    }

    private void Start()
    {
        spawnAudio = GameManager.instance.Sound.InitAudioSource(gameObject, "Ghost", false, 0.3f);
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange += OnDifficultyChange;
        }
    }

    /// <summary>
    /// Обработка события обновления сложности
    /// </summary>
    /// <param name="difficultyRate"></param>
    private void OnDifficultyChange(float difficultyRate)
    {
        // если сложность больше выставленного шага
        if (difficultyRate > (indexOnScene + 1) * difficultySpawnInterval)
        {
            if (spawnAudio != default)
            {
                spawnAudio.Play();
            }
            // размещаем привидение в случайной позиции
            ghosts[indexOnScene].PutOnScene(spawnPoints[Random.Range(0, spawnPoints.Length)].position, spawner.Gifts);
            indexOnScene++;
        }

        // отписываемся от события изменения сложности, если достигли максимального количества привидений
        if (indexOnScene >= maxCount)
        {
            if (GameManager.instance != default && GameManager.instance.Difficulty != default)
            {
                GameManager.instance.Difficulty.OnChange -= OnDifficultyChange;
            }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange -= OnDifficultyChange;
        }
    }

}
