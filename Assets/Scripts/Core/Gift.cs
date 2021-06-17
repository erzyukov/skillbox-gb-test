using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент логики подарка
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Gift : MonoBehaviour
{
    public delegate void PickUpAction(Gift gift);
    /// <summary>
    /// Событие срабатывающее при подборе подарка
    /// </summary>
    public event PickUpAction OnPickUp;

    public int ScoreAmount { get { return scoreAmount; } }
    public float ColliderRadius { get; private set; } = default;

    [HideInInspector]
    public int placeIndex;

    [SerializeField]
    [Tooltip("Количество очков за подарок")]
    private int scoreAmount = 1;

    [SerializeField]
    [Tooltip("Максимальное время заморозки при максимальной сложности")]
    private float maxFreezeTime = 5;

    [SerializeField]
    [Tooltip("Материал применяющийся при заморозке подарка")]
    private Material frozenMaterial = default;

    [Header("Ссылки на зависимые компоненты")]
    [SerializeField]
    private MeshRenderer meshRenderer = default;
    [SerializeField]
    private Collider interactCollider = default;
    [SerializeField]
    private Animator animator = default;

    private AudioSource freezeAudio = null;

    private Material defaultMaterial;

    private void Awake()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 360));
        gameObject.SetActive(false);
        transform.position = new Vector3(-5, -5, 0);
        ColliderRadius = gameObject.GetComponent<SphereCollider>().radius;

        defaultMaterial = meshRenderer.material;
    }

    private void Start()
    {
        freezeAudio = GameManager.instance.Sound.InitAudioSource(gameObject, "Freeze", false, 0.5f);
    }

    /// <summary>
    /// Размещает подарок на сцену в указанную позицию
    /// </summary>
    /// <param name="position">Позиция</param>
    public void PutOnScene(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Подбирает подарок
    /// </summary>
    public void PickUp()
    {
        // прячем подарок
        gameObject.SetActive(false);
        OnPickUp?.Invoke(this);
    }

    /// <summary>
    /// Обрабатывает подъем подарка противником
    /// </summary>
    public void EnemyPickUp()
    {
        StartCoroutine(Freeze());
    }

    /// <summary>
    /// Замораживает подарок на некоторое время
    /// </summary>
    private IEnumerator Freeze()
    {
        interactCollider.enabled = false;
        meshRenderer.material = frozenMaterial;
        float prevSpeed = animator.speed;
        animator.speed = 0;
        if (freezeAudio != default)
        {
            freezeAudio.Play();
        }

        // так же потом запустим здесь какой-нибудь эффект и звук

        float freezeTime;
        // время рассчитвается в зависимости от текущей сложности
        freezeTime = (GameManager.instance.Difficulty != default)
            ? maxFreezeTime * GameManager.instance.Difficulty.Current
            : maxFreezeTime;

        yield return new WaitForSeconds(freezeTime);
        interactCollider.enabled = true;
        meshRenderer.material = defaultMaterial;
        animator.speed = prevSpeed;
    }
}
