using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Компонент управляющий загрузкой звуковых ресурсов
/// </summary>
public class SoundManager : MonoBehaviour
{
    public enum group { SFX, Music };

    [SerializeField]
    [Tooltip("Ссылка на AudioMixer")]
    private AudioMixer mixer = default;

    [SerializeField]
    [Tooltip("Название группы спецэффектов AudioMixer")]
    private string sfxGroupName = "SFX";

    [SerializeField]
    [Tooltip("Название группы музыки AudioMixer")]
    private string musicGroupName = "Music";

    /// <summary>
    /// Подгружает аудио ресурс и прикрепляет компонент AudioSource на текущий объект и возвращает ссылку на него
    /// </summary>
    /// <param name="go">Объект, на который необходимо повесить компонент AudioSource</param>
    /// <param name="clipName">Имя аудио клипа в папке "/Resources/Audio/Player/"</param>
    /// <param name="isLoop">Необходимо ли зацикливать</param>
    /// <param name="volume">Громкость</param>
    /// <param name="mixerGroup">Тип группы AudioMixer</param>
    public AudioSource InitAudioSource(GameObject go, string clipName, bool isLoop, float volume = 0.65f, group mixerGroup = group.SFX)
    {
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("Audio/" + clipName);
        source.loop = isLoop;
        source.playOnAwake = false;
        source.volume = volume;

        string groupName = (mixerGroup == group.SFX) ? sfxGroupName : musicGroupName;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(groupName)[0];
        return source;
    }

}
