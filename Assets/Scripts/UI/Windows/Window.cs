using UnityEngine;

/// <summary>
/// Базовый компонент для окон
/// </summary>
[RequireComponent(typeof(Canvas))]
public class Window : MonoBehaviour
{
    [SerializeField] private GameObject window = default;

    private Canvas canvas;

    /// <summary>
    /// При активации прячет канвас окна
    /// Так же навешивает компоненту OnButtonOverCusor
    /// </summary>
    private void Awake()
    {
        canvas = gameObject.GetComponent<Canvas>();
        gameObject.AddComponent<ButtonEventsHandler>();
        canvas.enabled = false;
        window.SetActive(false);
    }

    /// <summary>
    /// Открывает окно
    /// </summary>
    public virtual void Open()
    {
        window.SetActive(true);
        canvas.enabled = true;
    }

    /// <summary>
    /// Закрывает окно
    /// </summary>
    public virtual void Close()
    {
        window.SetActive(false);
        canvas.enabled = false;
    }

    /// <summary>
    /// Обработка нажатия клавиши закрыть окно
    /// </summary>
    public virtual void CloseButtonHandler()
    {
        Close();
    }

}
