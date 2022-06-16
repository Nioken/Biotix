using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _uiSound;
    [SerializeField] private AudioClip _addUnitSound;
    private static AudioSource _audioSource;
    private static AudioManager _manager;

    private void OnEnable()
    {
        _manager = GetComponent<AudioManager>();
        _audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        CheckSettings();
    }

    public static void PlayUISound()
    {
        _audioSource.PlayOneShot(_manager._uiSound);
    }

    public static void PlayUnitAddSound()
    {
        _audioSource.PlayOneShot(_manager._addUnitSound);
    }

    public static void CheckSettings()
    {
        _manager._musicSource.mute = Settings.IsMusic;
        _audioSource.mute = Settings.IsSounds;
    }
}
