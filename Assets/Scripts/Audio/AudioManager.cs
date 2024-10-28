using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClip doorOpen;

    [SerializeField] AudioSource sfxPlayer;

    void Awake() {
        
        CreateInstance();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void PlaySFX(AudioClip audioClip) {

        sfxPlayer.PlayOneShot(audioClip);
    }
}
