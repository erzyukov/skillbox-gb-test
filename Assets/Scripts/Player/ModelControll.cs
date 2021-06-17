using UnityEngine;

/// <summary>
/// Компонент управления модельками игрока
/// </summary>
public class ModelControll : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Угол поворота модельки при движении влево")]
    private float leftLookAngle = 180f;

    [SerializeField]
    [Tooltip("Угол поворота модельки при движении вправо")]
    private float rightLookAngle = 0f;

    [SerializeField]
    [Tooltip("Моделька которой управляет игрок")]
    private GameObject model = default;

    [SerializeField]
    [Tooltip("Сумки отображающиеся в зависимости от сложности, должны стоять в порядке возрастания")]
    private GameObject[] bags = default;

    private float bagSizeStepSize;
    private byte currentBagSize = 0;

    private void Awake()
    {
        bagSizeStepSize = 1 / (float) bags.Length;
        for (int i = 0; i < bags.Length; i++)
        {
            bags[i].SetActive(false);
        }
    }

    private void Start()
    {
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange += OnDifficultyChange;
        }
        bags[currentBagSize].SetActive(true);
    }

    /// <summary>
    /// Поворачивает модельку вправо
    /// </summary>
    public void TurnRight()
    {
        Vector3 rotation = model.transform.eulerAngles;
        model.transform.rotation = Quaternion.Euler(rotation.x, leftLookAngle, rotation.x);
    }

    /// <summary>
    /// Поворачивает модельку влево
    /// </summary>
    public void TurnLeft()
    {
        Vector3 rotation = model.transform.eulerAngles;
        model.transform.rotation = Quaternion.Euler(rotation.x, rightLookAngle, rotation.x);
    }

    private void OnDifficultyChange(float difficultyRate)
    {
        // меняем рюкзак в зависимости от сложности
        if (difficultyRate > (currentBagSize + 1) * bagSizeStepSize)
        {
            bags[currentBagSize].SetActive(false);
            currentBagSize++;
            bags[currentBagSize].SetActive(true);
        }

        // если текущий размер больше количества размеров рюкзаков, отписываемся от событий
        if (currentBagSize >= bags.Length)
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
