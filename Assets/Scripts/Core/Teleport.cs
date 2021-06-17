using UnityEngine;

/// <summary>
/// Комонент управления телепортом
/// </summary>
public class Teleport : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ссылка на Transform телепорта выхода")]
    private Transform link = default;

    [SerializeField]
    [Tooltip("Тэг плеера")]
    private string playerTag = "Player";

    [SerializeField]
    [Tooltip("Отступ от телепорта выхода при появлении")]
    private float teleportOffset = 1.5f;

    private AudioSource source = null;

    private void Start()
    {
        source = GameManager.instance.Sound.InitAudioSource(gameObject, "Teleport", false, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // рассчитываем разницу между позициями телепорта и игрока
            float yDiff = transform.position.y - other.transform.position.y;
            // если отрицательная - зашли в нижний телепорт, значит выход ниже платформы
            // иначе в верхний и выход выше платформы
            Vector3 exit = new Vector3(
                other.transform.position.x, 
                (yDiff < 0)? link.transform.position.y - teleportOffset : link.transform.position.y + teleportOffset, 
                other.transform.position.z
            );

            if (source != default)
            {
                source.Play();
            }
            other.transform.position = exit;
        }
    }
}