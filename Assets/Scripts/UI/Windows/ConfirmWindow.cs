using UnityEngine.Events;

/// <summary>
/// Компонент управления окном подтверждения действия
/// </summary>
public class ConfirmWindow : Window
{
    private UnityAction YesAction;
    private UnityAction NoAction;

    /// <summary>
    /// Устанавливаем действие которое будет запускаться при нажатии на кнопку "да"
    /// </summary>
    /// <param name="action">Действие</param>
    public void SetYesAction(UnityAction action)
    {
        YesAction = action;
    }

    /// <summary>
    /// Устанавливаем действие которое будет запускаться при нажатии на кнопку "нет"
    /// </summary>
    /// <param name="action">Действие</param>
    public void SetNoAction(UnityAction action)
    {
        NoAction = action;
    }

    /// <summary>
    /// Обработка нажатия кнопки нет
    /// </summary>
    public void NoButtonHandler()
    {
        Close();
        NoAction();
    }

    /// <summary>
    /// Обработка нажатия кнопки да
    /// </summary>
    public void YesButtonHandler()
    {
        Close();
        YesAction();
    }

}
