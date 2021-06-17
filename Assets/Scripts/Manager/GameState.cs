using UnityEngine;

/// <summary>
/// Класс управляющий состоянием игры
/// </summary>
public class GameState
{
    /// <summary>
    /// Текущее состояние игрового процесса
    /// </summary>
    public GameStateType State { get; private set; }

    public delegate void GameStateAction(GameStateType type);
    /// <summary>
    /// Событие срабатывающее при изменении состоянии игры
    /// </summary>
    public event GameStateAction OnChange;

    private GameStateType previousState;


    public GameState()
    {
        StartGame();
    }

    /// <summary>
    /// Перейти в состояние Пауза (StateType.Pause)
    /// </summary>
    public void PauseGame()
    {
        if (State != GameStateType.Pause)
        {
            SetPreviousState();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UpdateState(GameStateType.Pause);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Продолжить игру после паузы
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateState(previousState);
    }

    /// <summary>
    /// Проверяет разрешина ли пауза
    /// </summary>
    /// <returns></returns>
    public bool IsPauseAllow()
    {
        if (State == GameStateType.GameOver || State == GameStateType.None)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Проверяет разрешино ли действие
    /// </summary>
    /// <returns></returns>
    public bool IsActionAllow()
    {
        if (State == GameStateType.Game)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Начинает игру
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1;
        UpdateState(GameStateType.Game);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Закончить игру
    /// </summary>
    public void FinishGame(bool isStopTime = false)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UpdateState(GameStateType.GameOver);
        if (isStopTime)
        {
            StopTime();
        }
    }

    /// <summary>
    /// Остановить игровое время
    /// </summary>
    public void StopTime()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Изменяет состояние на указанное. И вызывает событие о смене состояния.
    /// </summary>
    /// <param name="state">Тип состояния</param>
    private void UpdateState(GameStateType state)
    {
        this.State = state;
        OnChange?.Invoke(state);
    }

    /// <summary>
    /// Устанавливает предыдущее состояние
    /// Пока может устанавливаться только StateType.Game
    /// Остальные пока не нужны. Функция для удобства в будущем.
    /// </summary>
    private void SetPreviousState()
    {
        previousState = State;
    }

}
