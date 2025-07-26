using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterAnimator : MonoBehaviour
{
    private Animator animator;
    
    void Awake() => animator = GetComponent<Animator>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterAnimator(animator);

    // TODO ASYNC
    private IEnumerator RegisterWhenReady()
    {
        while (GameController.Instance == null)
        {
            Debug.Log("[AutoregisterAnimator] GameController.Instance is null");
            yield return null;
        }
        
        Debug.Log("[AutoregisterAnimator] animator registered");
        GameController.Instance.RegisterAnimator(animator);
    }
}
