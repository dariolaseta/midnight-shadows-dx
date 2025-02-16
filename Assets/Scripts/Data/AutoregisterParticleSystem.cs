using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;
    
    void Awake() => ps = GetComponent<ParticleSystem>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterParticleSystem(ps);
    
    private IEnumerator RegisterWhenReady()
        {
            while (GameController.Instance == null)
            {
                yield return null;   
            }
            
            GameController.Instance.RegisterParticleSystem(ps);
        }
}
