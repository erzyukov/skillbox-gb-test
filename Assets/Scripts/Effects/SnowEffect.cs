using UnityEngine;

/// <summary>
/// Компонент управления системой частиц снега в зависимости от сложности игры
/// </summary>
public class SnowEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Минимальное количество частиц при минимальой сложности")]
    private float minEmissionCount = 30;
    [SerializeField]
    [Tooltip("Максимальное количество частиц при максимальной сложности")]
    private float maxEmissionCount = 300;

    [SerializeField]
    [Tooltip("Длительность жизни при минимальой сложности")]
    private float minEmissionLifetime = 6;
    [SerializeField]
    [Tooltip("Длительность жизни при максимальной сложности")]
    private float maxEmissionLifetime = 3;

    [SerializeField]
    [Tooltip("Скорость частиц при минимальой сложности")]
    private float minEmissionSpeed = 5;
    [SerializeField]
    [Tooltip("Скорость частиц при максимальной сложности")]
    private float maxEmissionSpeed = 10;

    [SerializeField]
    [Tooltip("Сила шума частиц при минимальой сложности")]
    private float minEmissionNoise = 1;
    [SerializeField]
    [Tooltip("Сила шума частиц при максимальной сложности")]
    private float maxEmissionNoise = 3;

    [SerializeField]
    [Tooltip("Ссылка на систему частиц снега")]
    private ParticleSystem particles = default;

    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.NoiseModule noise;
    private ParticleSystem.MainModule main;

    private void Awake()
    {
        emission = particles.emission;
        main     = particles.main;
        noise    = particles.noise;
    }

    private void Start()
    {
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange += OnDifficultyChange;
        }
    }

    private void OnDifficultyChange(float difficultyRate)
    {
        emission.rateOverTime   = minEmissionCount + (maxEmissionCount - minEmissionCount) * difficultyRate;
        main.startLifetime      = minEmissionLifetime + (maxEmissionLifetime - minEmissionLifetime) * difficultyRate;
        main.startSpeed         = minEmissionSpeed + (maxEmissionSpeed - minEmissionSpeed) * difficultyRate;
        noise.strength          = minEmissionNoise + (maxEmissionNoise - minEmissionNoise) * difficultyRate;
    }


    private void OnDestroy()
    {
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange -= OnDifficultyChange;
        }
    }

}
