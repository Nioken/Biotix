using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip _uiSound;
    [SerializeField] private AudioClip _addUnitSound;
    private static AudioSource _audioSource;
    private static AudioManager _manager;

    private void Start()
    {
        _manager = GetComponent<AudioManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayUISound()
    {
        _audioSource.PlayOneShot(_manager._uiSound);
    }

    public static void PlayUnitAddSound()
    {
        _audioSource.PlayOneShot(_manager._addUnitSound);
    }
}
