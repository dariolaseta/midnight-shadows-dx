using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterAudiosource : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Awake() => audioSource = GetComponent<AudioSource>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterAudioSource(audioSource);
    
    private IEnumerator RegisterWhenReady()
    {
        while (GameController.Instance == null)
        {
            yield return null;   
        }
        
        GameController.Instance.RegisterAudioSource(audioSource);
    }
}
