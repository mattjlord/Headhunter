using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private PlayerOrganism _playerInstance;
    [SerializeField] private GameObject _audioPrefabInstance;

    private static PlayerOrganism _player;
    private static GameObject _audioPrefab;

    private void Start()
    {
        _player = _playerInstance;
        _audioPrefab = _audioPrefabInstance;
    }

    public static void PlaySound(AudioClip audioClip, Vector3 position, float audibleDistance)
    {
        GameObject audioInstance = Instantiate(_audioPrefab, position, Quaternion.identity);
        
        AudioSource audioSource = audioInstance.GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.maxDistance = audibleDistance;
        audioSource.Play();

        Destroy(audioInstance, audioClip.length + 0.1f);
    }
}
