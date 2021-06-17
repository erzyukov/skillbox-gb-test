using System.Linq;
using UnityEngine;

/// <summary>
/// Компонент размещения подарков на игровой сцене
/// </summary>
public class GiftSpawner : MonoBehaviour
{
    public Gift[] Gifts { get { return gifts; } }

    /// <summary>
    /// Места для спауна подарков
    /// </summary>
    [Tooltip("Места для спауна подарков")]
    [SerializeField] private BoxCollider[] spawnPlaces = default;

    /// <summary>
    /// Одновременное количество подарков на сцене
    /// </summary>
    [Tooltip("Одновременное количество подарков на сцене, если количетсво будет более чем в два раза чем мест, то подарки могут накладываться.")]
    [SerializeField] private int maxCount = 10;

    private Gift[] gifts;
    private int[] placeGiftCounts;

    private void Awake()
    {
        placeGiftCounts = new int[maxCount];
        GameObject prefab = Resources.Load<GameObject>("Core/Gift");
        
        // инициализируем необходимое количество подарков и распологаем их на сцене
        gifts = new Gift[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            GameObject obj = Instantiate<GameObject>(prefab);
            gifts[i] = obj.GetComponent<Gift>();
            gifts[i].OnPickUp += OnGiftPickUp;
            PutGiftOnScene(gifts[i]);
        }
    }

    /// <summary>
    /// Размещает указанный подарок на сцене
    /// </summary>
    /// <param name="gift">Подарок</param>
    private void PutGiftOnScene(Gift gift)
    {
        int placeIndex = GetPlaceIndex();
        gift.placeIndex = placeIndex;
        // увеличиваем количество подарков на выбранном месте
        placeGiftCounts[placeIndex]++;
        gift.PutOnScene(GetPlacePosition(placeIndex));
    }

    /// <summary>
    /// Возвращает случайное место для подарка
    /// </summary>
    /// <returns>Индекс места</returns>
    private int GetPlaceIndex()
    {
        int maxIterationCount = 20;
        int i = 0;
        int placeIndex;

        // выбераем место до тех пор, пока оно не будет удовлетворять необходимой плотности
        // для безопасности вводим максимальное количество итераций
        do
        {
            i++;
            placeIndex = Random.Range(0, spawnPlaces.Length);
        } while (IsPlaseSaturated(placeIndex) && i < maxIterationCount);

        return placeIndex;
    }

    /// <summary>
    /// Проверяет, насыщено ли текущее место подарками
    /// </summary>
    /// <param name="index">Индекс места</param>
    /// <returns>Насыщенность</returns>
    private bool IsPlaseSaturated(int index)
    {
        // получаем сумму всех подарков расположенных на сцена
        int sum = placeGiftCounts.Sum();
        // Вычесляем максимальную плотность подарков на место
        float maxDensity = maxCount / spawnPlaces.Length;
        // получаем количество подарков на указанном месте
        int currentPlaceGiftCounts = placeGiftCounts[index];
        // если плотность ниже единицы (мест меньше чем подарков)
        if (maxDensity < 1)
        {
            // из текущего количества подарков вычетаем отношение суммы подарков на сцене к количеству мест
            currentPlaceGiftCounts -= sum / spawnPlaces.Length;
        }
        return currentPlaceGiftCounts > maxDensity;
    }

    /// <summary>
    /// Возвращает случайную позицию в заданном месте
    /// </summary>
    /// <param name="placeIndex">Индекс места</param>
    /// <returns>Координаты позиции</returns>
    private Vector3 GetPlacePosition(int placeIndex)
    {
        Vector3 result;
        int maxIterationCount = 50;
        int i = 0;

        bool isFree;

        // выбераем случайную позицию и проверяем не будет ли пересекаться подарок с другими в этом месте
        // для безопасности вводим максимальное количество итераций
        do
        {
            i++;
            BoxCollider spawnPlace = spawnPlaces[placeIndex];
            Vector3 center = spawnPlace.bounds.center;
            Vector3 size = spawnPlace.bounds.size;
            float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
            float z = center.z;
            result = new Vector3(x, y, z);
            isFree = IsFreePosition(result, placeIndex);
        } while (!isFree && i < maxIterationCount);

        return result;
    }

    /// <summary>
    /// Проверяет свободна ли выбранная позиция в выбранном месте
    /// </summary>
    /// <param name="position">Координаты позиции</param>
    /// <param name="placeIndex">Индекс места</param>
    /// <returns>Свободно ли место</returns>
    private bool IsFreePosition(Vector3 position, int placeIndex)
    {
        bool result = true;

        for(int i = 0; i < maxCount; i++)
        {
            if (gifts[i] != default && gifts[i].placeIndex == placeIndex)
            {
                float xDistance = Mathf.Abs(position.x - gifts[i].transform.position.x);
                float yDistance = Mathf.Abs(position.y - gifts[i].transform.position.y);

                if (xDistance <= 2 * gifts[i].ColliderRadius || yDistance <= 2 * gifts[i].ColliderRadius)
                {
                    result = false;
                }
            }
        }

        return result;
    }

    private void OnGiftPickUp(Gift gift)
    {
        GameManager.instance.Score.Increase(gift.ScoreAmount);
        //уменьшаем количество подарков на месте gift.placeIndex
        placeGiftCounts[gift.placeIndex]--;
        PutGiftOnScene(gift);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (gifts[i] != default)
            {
                gifts[i].OnPickUp -= OnGiftPickUp;
            }
        }
    }
}
