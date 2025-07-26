using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterAudiosource : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Awake() => audioSource = GetComponent<AudioSource>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterAudioSource(audioSource);
    
    // TODO ASYNC
    private IEnumerator RegisterWhenReady()
    {
        while (GameController.Instance == null)
        {
            Debug.Log("[AutoregisterAudiosource] GameController.Instance is null");
            yield return null;   
        }
        
        Debug.Log("[AutoregisterAudiosource] Audiosource registered");
        GameController.Instance.RegisterAudioSource(audioSource);
    }
}
