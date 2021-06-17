using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Компонент смены курсора при наведении на кнопку
/// </summary>
public class ButtonEventsHandler : MonoBehaviour
{
    /// <summary>
    /// Курсор для состояния наведение на интерактивный объект
    /// </summary>
    private Texture2D onOverCursor = default;

    /// <summary>
    /// Звуковой эффект наведения мыши на кнопку
    /// </summary>
    private AudioSource onOverSource = null;

    /// <summary>
    /// Звуковой эффект нажатия мыши на кнопку
    /// </summary>
    private AudioSource onClickSource = null;

    private void Awake()
    {
        onOverCursor = Resources.Load<Texture2D>("UI/Cursors/Pointer");

        InitSound();

        // находим все кнопки в окне и добавляем необходимые события
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            AddDeligate(ref trigger, EventTriggerType.PointerEnter, () => { OnMouseOver(); });
            AddDeligate(ref trigger, EventTriggerType.PointerExit, () => { OnMouseExit(); });
            AddDeligate(ref trigger, EventTriggerType.PointerUp, () => { OnMouseExit(); });
            AddDeligate(ref trigger, EventTriggerType.PointerClick, () => { OnMouseClick(); });
        }
    }

    /// <summary>
    /// Смена курсора при наведении мыши
    /// </summary>
    /// <param name="data">Данные события</param>
    public void OnMouseOver()
    {
        Cursor.SetCursor(onOverCursor, Vector2.zero, CursorMode.Auto);
        onOverSource.Play();
    }

    /// <summary>
    /// Смена курсора при выходе мыши
    /// </summary>
    /// <param name="data">Данные события</param>
    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Обработка нажатия клавиши на кнопку
    /// </summary>
    /// <param name="data"></param>
    public void OnMouseClick()
    {
        onClickSource.Play();
    }

    /// <summary>
    /// Подгружает необходимые звуковые ресурсы
    /// </summary>
    private void InitSound()
    {
        AudioClip clipOver = Resources.Load<AudioClip>("Audio/UI/Over");
        AudioClip clipClick = Resources.Load<AudioClip>("Audio/UI/Click");

        if (clipOver != null)
        {
            onOverSource = gameObject.AddComponent<AudioSource>();
            onOverSource.clip = clipOver;
            onOverSource.loop = false;
            onOverSource.volume = 0.5f;
        }

        if (clipClick != null)
        {
            onClickSource = gameObject.AddComponent<AudioSource>();
            onClickSource.clip = clipClick;
            onClickSource.loop = false;
            onClickSource.volume = 0.5f;
        }
    }

    /// <summary>
    /// Добавляет обработчик на указанный тип события
    /// </summary>
    /// <param name="trigger">Триггер события</param>
    /// <param name="eventType">Тип события</param>
    /// <param name="action">Обработчик события</param>
    private void AddDeligate(ref EventTrigger trigger, EventTriggerType eventType, UnityAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => { action(); });
        trigger.triggers.Add(entry);
    }

}