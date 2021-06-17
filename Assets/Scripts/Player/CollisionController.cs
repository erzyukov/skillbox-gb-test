using UnityEngine;

/// <summary>
/// Компонент управления столкновения игроком с объектами мира
/// </summary>
public class CollisionController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Имя тэга объекта подарка")]
    private string giftTag = "Gift";

    [SerializeField]
    [Tooltip("Имя тэга объекта препятствий")]
    private string obstacleTag = "Obstacle";

    [SerializeField]
    [Tooltip("Имя тэга объекта привидений")]
    private string ghostTag = "Ghost";

    [SerializeField]
    [Tooltip("Ссылка на объект компонент эффектов игрока")]
    private EffectsController effects = default;

    /// <summary>
    /// Компонент PlayerAudioController игрока
    /// </summary>
    [SerializeField]
    private PlayerAudioController audioController = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(giftTag))
        {
            audioController.TakeGift();
            effects.GiftPickUp();
            other.gameObject.GetComponent<Gift>().PickUp();
        }
        
        if (other.CompareTag(obstacleTag))
        {
            audioController.Crash();
            GameManager.instance.State.FinishGame();
        }

        if (other.CompareTag(ghostTag))
        {
            audioController.Scream();
        }
    }

}
