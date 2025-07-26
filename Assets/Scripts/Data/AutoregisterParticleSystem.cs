using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;
    
    void Awake() => ps = GetComponent<ParticleSystem>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterParticleSystem(ps);
    
    // TODO ASYNC
    private IEnumerator RegisterWhenReady()
        {
            while (GameController.Instance == null)
            {
                Debug.Log("[AutoregisterPartycleSystem] GameController.Instance is null");
                yield return null;
            }
            
            Debug.Log("[AutoregisterPartycleSystem] PartycleSystem registered");
            GameController.Instance.RegisterParticleSystem(ps);
        }
}
