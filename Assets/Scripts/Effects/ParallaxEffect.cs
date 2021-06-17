using UnityEngine;

/// <summary>
/// Компонент реализующий эффект параллакса для фона
/// </summary>
//[RequireComponent(typeof(Collider))]
public class ParallaxEffect : MonoBehaviour
{
    /// <summary>
    /// Максимальный размер смещения фона
    /// </summary>
    [SerializeField]
    private float parallaxSize = 1;

    /// <summary>
    /// Ссылка на компонент Collider для получения размера обхекта фона
    /// </summary>
    [SerializeField]
    private Collider col = default;

    private Transform target;
    private float xSize;
    private float ySize;
    private float xCenter;
    private float yCenter;

    private void Start()
    {
        if (GameManager.instance.Resources != default && GameManager.instance.Resources.Player)
        {
            target = GameManager.instance.Resources.Player.transform;
        }
        else
        {
            gameObject.SetActive(false);
        }

        // определяем размеры и центр объекта фона
        xSize = col.bounds.size.x;
        ySize = col.bounds.size.y;
        xCenter = col.bounds.center.x;
        yCenter = col.bounds.center.y;
    }

    private void FixedUpdate()
    {
        // рассчитываем новые координаты центра фона
        float newX = -(target.position.x - xCenter) / xSize * 2 * parallaxSize + xCenter;
        float newY = - (target.position.y - yCenter) / ySize * 2 * parallaxSize + yCenter;

        // сдвигаем бакграунд в зависимости от положения цели для параллакс эффекта
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
