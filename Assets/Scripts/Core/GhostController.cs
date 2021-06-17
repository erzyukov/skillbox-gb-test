using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент управления поведением привидения
/// </summary>
public class GhostController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Время задержки перед поиском подарка")]
    private float searchDelay = 1.5f;

    [SerializeField]
    [Tooltip("Скорость привидения")]
    private float speed = 0.1f;

    [SerializeField]
    [Tooltip("Скорость при максимальной сложности")]
    private float maxDifficultySpeed = 0.3f;

    private Gift[] gifts = default;
    private Gift target = default;
    private float currentSpeed;
    private float journeyTime;
    private float journeyLength;
    private Vector3 destination;

    private void Start()
    {
        currentSpeed = speed;
        gameObject.SetActive(false);
        // до активации размещаем за сценой
        transform.position = new Vector3(-5, -5, 0);
        destination = Vector3.zero;
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange += OnDifficultyChange;
        }
    }

    private void FixedUpdate()
    {
        // перемещение привидения к цели
        if (target != default && destination != default)
        {
            float distCovered = journeyTime * currentSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(transform.position, destination, fractionOfJourney);
            journeyTime += Time.fixedUnscaledDeltaTime;
        }
    }

    /// <summary>
    /// Размещает привидение на сцену в указанную позицию и запускает логику
    /// </summary>
    /// <param name="position">Позиция</param>
    public void PutOnScene(Vector3 position, Gift[] gifts)
    {
        this.gifts = gifts;
        gameObject.SetActive(true);
        transform.position = position;
        StartCoroutine(SearchGift());
        StartCoroutine(CheckGiftAbsent());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gift"))
        {
            // замораживаем все подарки по пути
            other.gameObject.GetComponent<Gift>().EnemyPickUp();
            // если достигли цели - ищем следующий подарок
            if (target != default && other.transform == target.transform)
            {
                destination = Vector3.zero;
                StartCoroutine(SearchGift());
            }
        }
    }

    /// <summary>
    /// Выберает случайный подарок
    /// </summary>
    private void ChooseGift()
    {
        // выбераем случайный подарок
        target = gifts[Random.Range(0, gifts.Length)];
        journeyTime = 0;
        journeyLength = Vector3.Distance(transform.position, target.transform.position);
        destination = target.transform.position;
    }

    /// <summary>
    /// Запускает поиск следующего подарка с задержкой
    /// </summary>
    private IEnumerator SearchGift()
    {
        target = default;
        yield return new WaitForSeconds(searchDelay);
        ChooseGift();
    }

    /// <summary>
    /// Проверяет на месте ли еще подарок, за которым привидение летело
    /// Используется для оптимизации, чтобы не делать проверку каждый кадр
    /// </summary>
    private IEnumerator CheckGiftAbsent()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (destination != Vector3.zero && target != default && target.transform.position != destination)
            {
                StartCoroutine(SearchGift());
            }
        }
    }

    /// <summary>
    /// Обработка события обновления сложности
    /// </summary>
    /// <param name="difficultyRate"></param>
    private void OnDifficultyChange(float difficultyRate)
    {
        currentSpeed = speed + difficultyRate * (maxDifficultySpeed - speed);
    }

    private void OnDestroy()
    {
        if (GameManager.instance != default && GameManager.instance.Difficulty != default)
        {
            GameManager.instance.Difficulty.OnChange -= OnDifficultyChange;
        }
    }

}
