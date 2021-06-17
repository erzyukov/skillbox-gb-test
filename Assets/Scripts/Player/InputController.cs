using UnityEngine;

/// <summary>
/// Компонент отвечающий за контроллер игрока
/// </summary>
[RequireComponent(typeof(DeerController))]
public class InputController : MonoBehaviour
{
    [Header("Настройки контроллера игрока")]
    /// <summary>
    /// Клавиша вверх
    /// </summary>
    [SerializeField]
    [Tooltip("Клавиша вверх")]
    private KeyCode upKey = default;

    /// <summary>
    /// Клавиша влево
    /// </summary>
    [SerializeField]
    [Tooltip("Клавиша влево")]
    private KeyCode leftKey = default;

    /// <summary>
    /// Клавиша вправо
    /// </summary>
    [SerializeField]
    [Tooltip("Клавиша вправо")]
    private KeyCode rightKey = default;

    [Header("Ссылки на компоненты")]
    /// <summary>
    /// Ссылка на компонент управляющий санями
    /// </summary>
    [SerializeField]
    [Tooltip("Ссылка на компонент игрока SleighController")]
    private DeerController deer = null;

    [SerializeField]
    [Tooltip("Ссылка на компонент игрока ModelControll")]
    private ModelControll model = null;

    private void FixedUpdate()
    {
        if (GameManager.instance != null && !GameManager.instance.State.IsActionAllow())
        {
            return;
        }

        // обрабатываем нажатие клавиши вперед
        if (Input.GetKey(upKey))
        {
            deer.AddUpPower();
        }
        // обрабатываем нажатие клавиши влево
        if (Input.GetKey(leftKey))
        {
            deer.AddLeftPower();
            model.TurnLeft();
        }
        // обрабатываем нажатие клавиши вправо
        else if (Input.GetKey(rightKey))
        {
            deer.AddRightPower();
            model.TurnRight();
        }
    }
}