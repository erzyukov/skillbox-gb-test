using UnityEngine;

/// <summary>
/// Компонент для управления Rigidbody
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class DeerController : MonoBehaviour
{
    public float Speed
    {
        get
        {
            //return rb.velocity.magnitude;
            float value = (GameManager.instance == null || GameManager.instance.State.IsActionAllow())
                ? rb.velocity.magnitude
                : velocityMemory.magnitude;
            return (value > 0.1f) ? value : 0;
        }
    }

    [Header("Настройки мощности саней")]

    [Tooltip("Мощность саней")]
    [SerializeField] private float power = 20;

    [Space]

    [Tooltip("Масса саней")]
    [SerializeField] private float mass = 1f;

    [Tooltip("Масса саней на максимальной сложности")]
    [SerializeField] private float maxDifficultyMass = 1f;

    [Tooltip("Угол боковой силы")]
    [Range(0, 90)]
    [SerializeField] private byte sidePowerAngle = 45;

    [Space]
    [Header("Ссылки на зависимые компоненты")]
    [SerializeField] private Rigidbody rb = null;

    private Vector3 velocityMemory;

    private void Awake()
    {
        // инициализация значений стандартных компонент
        // устанавливается масса для компоненты Rigidbody
        rb.mass = mass;

        if (GameManager.instance != null)
        {
            if (GameManager.instance.State != null)
            {
                GameManager.instance.State.OnChange += OnGameStateChange;
            }
            if (GameManager.instance.Difficulty != null)
            {
                GameManager.instance.Difficulty.OnChange += OnGameDifficultyChange;
            }
        }
    }

    /// <summary>
    /// Задает вертикальную силу
    /// </summary>
    public void AddUpPower()
    {
        rb.AddForce(Vector3.up * power);
    }

    /// <summary>
    /// Задает левую силу
    /// </summary>
    public void AddLeftPower()
    {
        AddSidePower(-1);
    }

    /// <summary>
    /// Задает правую силу
    /// </summary>
    public void AddRightPower()
    {
        AddSidePower(1);
    }

    private void AddSidePower(sbyte dir)
    {
        // рассчитываем вектор по углу
        float angleRad = sidePowerAngle * (Mathf.PI / 180f);
        Vector3 vectorDir = new Vector3(dir * Mathf.Sin(angleRad), Mathf.Cos(angleRad), 0);

        rb.AddForce(vectorDir * power);
    }

    private void OnGameStateChange(GameStateType type)
    {
        switch (type)
        {
            case GameStateType.GameOver:
                rb.isKinematic = true;
                break;
        }
    }

    private void OnGameDifficultyChange(float difficulty)
    {
        rb.mass = mass + difficulty * (maxDifficultyMass - mass);
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.State != null)
            {
                GameManager.instance.State.OnChange -= OnGameStateChange;
            }
            if (GameManager.instance.Difficulty != null)
            {
                GameManager.instance.Difficulty.OnChange -= OnGameDifficultyChange;
            }
        }
    }

}
