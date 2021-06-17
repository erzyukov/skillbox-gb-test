using UnityEngine;

/// <summary>
/// Компонента для управления звуками игрока
/// </summary>
public class PlayerAudioController : MonoBehaviour
{
    /// <summary>
    /// Ссылка на аудио-ресурс бега оленя
    /// </summary>
    private AudioSource horse;

    /// <summary>
    /// Ссылка на аудио-ресурс поднятия подарка
    /// </summary>
    private AudioSource takeGift;

    /// <summary>
    /// Ссылка на аудио-ресурс столкновения
    /// </summary>
    private AudioSource crash;

    /// <summary>
    /// Ссылка на аудио-ресурс крика
    /// </summary>
    private AudioSource scream;

    /// <summary>
    /// Метод загрузки всех необходимых ресурсов и начальной работы с ними
    /// </summary>
    private void Awake()
    {
    }

    private void Start()
    {
        // инициализация звуков игрока
        horse = GameManager.instance.Sound.InitAudioSource(gameObject, "Player/Horse", true, 0.4f);
        takeGift = GameManager.instance.Sound.InitAudioSource(gameObject, "Player/TakeGift", false, 0.6f);
        crash = GameManager.instance.Sound.InitAudioSource(gameObject, "Player/Crash", false, 0.8f);
        scream = GameManager.instance.Sound.InitAudioSource(gameObject, "Player/Scream", false, 1f);

        OnGame();

        if (GameManager.instance != default && GameManager.instance.State != default)
        {
            GameManager.instance.State.OnChange += OnGameStateChange;
        }
    }

    /// <summary>
    /// Проигрывает аудио подъема подарка
    /// </summary>
    public void TakeGift()
    {
        if (takeGift != default)
        {
            takeGift.Play();
        }
    }

    /// <summary>
    /// Проигрывает аудио столкновения
    /// </summary>
    public void Crash()
    {
        if (crash != default)
        {
            crash.Play();
        }
    }

    /// <summary>
    /// Проигрывает аудио крика
    /// </summary>
    public void Scream()
    {
        if (scream != default && !scream.isPlaying)
        {
            scream.Play();
        }
    }

    /// <summary>
    /// Обрабатывает начало игры
    /// </summary>
    private void OnGame()
    {
        if (horse != default)
        {
            horse.Play();
        }
    }

    /// <summary>
    /// Обрабатывает паузу игры
    /// </summary>
    private void OnPause()
    {
        if (horse != default)
        {
            horse.Pause();
        }
    }

    /// <summary>
    /// Обработка изменения состояния игры
    /// </summary>
    /// <param name="type">Тип состояния</param>
    private void OnGameStateChange(GameStateType type)
    {
        switch (type)
        {
            case GameStateType.Game:
                OnGame();
                break;
            case GameStateType.Pause:
            case GameStateType.GameOver:
                OnPause();
                break;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != default && GameManager.instance.State != default)
        {
            GameManager.instance.State.OnChange -= OnGameStateChange;
        }
    }

}
