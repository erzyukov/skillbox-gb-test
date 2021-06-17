using System.Collections;
using UnityEngine;

/// <summary>
/// Компонента тряски камеры при проигрыше
/// </summary>
public class CameraShake : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Длительность тряски камеры")]
    private float shakeDuration = 0f;

    [SerializeField]
    [Tooltip("Амплитуда тряски камеры")]
    private float shakeAmount = 0.7f;

    private Vector3 defaultPosition;

    private void Awake()
    {
        defaultPosition = transform.position;
    }

    private void Start()
    {
        if (GameManager.instance != null && GameManager.instance.State != default)
        {
            GameManager.instance.State.OnChange += OnGameStateChange;
        }
    }

    /// <summary>
    /// Делает тряску
    /// </summary>
    private IEnumerator DoShake()
    {
        float duration = shakeDuration;
        while(duration > 0)
        {
            // придаем случайное положение каждый кадр
            transform.position = defaultPosition + Random.insideUnitSphere * shakeAmount;
            duration -= Time.fixedUnscaledDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        // возвращаем камеру на место
        transform.position = defaultPosition;
        yield return new WaitForFixedUpdate();
        GameManager.instance.State.StopTime();
    }

    /// <summary>
    /// Обработчик события смены состояния игры
    /// </summary>
    /// <param name="type">Тип состояния игры</param>
    private void OnGameStateChange(GameStateType type)
    {
        switch (type)
        {
            case GameStateType.GameOver:
                StartCoroutine(DoShake());
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
